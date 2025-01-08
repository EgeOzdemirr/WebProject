using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;
using WebProject.Catalog.Dtos.ProductDtos;
using WebProject.Catalog.Services.ProductRecommendationServices;

namespace WebProject.Catalog.Services
{
    public class ProductRecommendationService : IProductRecommendationService
    {
        private readonly IMongoDatabase _mongoDb;
        private readonly string _sqlConnectionString;
        private readonly Random _random;

        public ProductRecommendationService(IMongoDatabase mongoDb, IConfiguration configuration)
        {
            _mongoDb = mongoDb;
            _sqlConnectionString = configuration.GetConnectionString("OrderingDb");
            _random = new Random();
        }

        private async Task<HashSet<string>> GetUserOrderedProductsAsync(string userId)
        {
            var orderedProducts = new HashSet<string>();

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    SELECT DISTINCT od.ProductName
                    FROM Orderings o
                    JOIN OrderDetails od ON o.OrderingId = od.OrderingId
                    WHERE o.UserId = @UserId", connection);

                command.Parameters.AddWithValue("@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    orderedProducts.Add(reader.GetString(0));
                }
            }

            return orderedProducts;
        }

        private HashSet<string> GetKeywords(string description)
        {
            if (string.IsNullOrEmpty(description))
                return new HashSet<string>();

            var stopWords = new HashSet<string> { "ve", "ile", "bir", "bu", "için", "de", "da", "mi", "mı" };
            return new HashSet<string>(
                description.ToLower()
                    .Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(word => !stopWords.Contains(word))
            );
        }

        private int CalculateSimilarity(BsonDocument product1, BsonDocument product2)
        {
            var score = 0;

            // Kategori benzerliği
            if (product1["CategoryId"] == product2["CategoryId"])
                score += 2;

            // Açıklama benzerliği
            var desc1Keywords = GetKeywords(product1["ProductDescription"].AsString);
            var desc2Keywords = GetKeywords(product2["ProductDescription"].AsString);
            score += desc1Keywords.Intersect(desc2Keywords).Count();

            return score;
        }

        private async Task<List<ResultProductDto>> GetRandomProductsAsync(int count, HashSet<string> excludeIds = null)
        {
            var productsCollection = _mongoDb.GetCollection<BsonDocument>("Products");
            var filter = excludeIds != null && excludeIds.Any()
                ? Builders<BsonDocument>.Filter.Nin("_id", excludeIds.Select(id => ObjectId.Parse(id)))
                : Builders<BsonDocument>.Filter.Empty;

            var products = await productsCollection.Find(filter).ToListAsync();
            return products.OrderBy(x => _random.Next())
                         .Take(count)
                         .Select(p => new ResultProductDto
                         {
                             ProductId = p["_id"].AsObjectId.ToString(),
                             ProductName = p["ProductName"].AsString,
                             ProductPrice = ConvertToDecimal(p["ProductPrice"]),
                             ProductImageUrl = p["ProductImageUrl"].AsString,
                             ProductDescription = p["ProductDescription"].AsString,
                             CategoryId = p["CategoryId"].AsObjectId.ToString()
                         })
                         .ToList();
        }

        private decimal ConvertToDecimal(BsonValue value)
        {
            try
            {
                if (value.IsString)
                {
                    if (decimal.TryParse(value.AsString, out decimal result))
                        return result;
                    return 0;
                }
                else if (value.IsDouble)
                {
                    return (decimal)value.AsDouble;
                }
                else if (value.IsDecimal128)
                {
                    return (decimal)value.AsDecimal128;
                }
                else if (value.IsInt32)
                {
                    return value.AsInt32;
                }
                else if (value.IsInt64)
                {
                    return value.AsInt64;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<List<ResultProductDto>> GetRecommendedProductsAsync(string userId, int count = 4)
        {
            try
            {
                var productsCollection = _mongoDb.GetCollection<BsonDocument>("Products");

                // Kullanıcının aldığı ürünleri bul
                var orderedProductNames = await GetUserOrderedProductsAsync(userId);
                if (!orderedProductNames.Any())
                {
                    // Kullanıcının siparişi yoksa rastgele ürünler öner
                    return await GetRandomProductsAsync(count);
                }

                // Son alınan ürünü bul
                var lastProduct = await productsCollection.Find(
                    Builders<BsonDocument>.Filter.In("ProductName", orderedProductNames)
                ).FirstOrDefaultAsync();

                if (lastProduct == null)
                    return await GetRandomProductsAsync(count);

                // Benzer ürünleri bul
                var allProducts = await productsCollection.Find(
                    Builders<BsonDocument>.Filter.Nin("ProductName", orderedProductNames)
                ).ToListAsync();

                var productScores = allProducts
                    .Select(p => new { Product = p, Score = CalculateSimilarity(lastProduct, p) })
                    .Where(x => x.Score > 0)
                    .OrderByDescending(x => x.Score)
                    .Take(count)
                    .ToList();

                var recommendations = new List<ResultProductDto>();

                // Benzer ürünleri ekle
                foreach (var item in productScores)
                {
                    recommendations.Add(new ResultProductDto
                    {
                        ProductId = item.Product["_id"].AsObjectId.ToString(),
                        ProductName = item.Product["ProductName"].AsString,
                        ProductPrice = ConvertToDecimal(item.Product["ProductPrice"]),
                        ProductImageUrl = item.Product["ProductImageUrl"].AsString,
                        ProductDescription = item.Product["ProductDescription"].AsString,
                        CategoryId = item.Product["CategoryId"].AsObjectId.ToString()
                    });
                }

                // Eğer yeterli benzer ürün yoksa, rastgele ürünlerle tamamla
                if (recommendations.Count < count)
                {
                    var existingIds = recommendations.Select(r => r.ProductId).ToHashSet();
                    var randomProducts = await GetRandomProductsAsync(count - recommendations.Count, existingIds);
                    recommendations.AddRange(randomProducts);
                }

                return recommendations;
            }
            catch (Exception ex)
            {
                // Hata logla
                Console.WriteLine($"Öneri oluşturma hatası: {ex.Message}");
                return await GetRandomProductsAsync(count);
            }
        }
    }
}