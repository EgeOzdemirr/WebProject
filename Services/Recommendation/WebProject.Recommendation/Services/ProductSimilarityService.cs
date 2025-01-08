using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.Recommendation.Services
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        // Add other properties as needed (price, image, etc.)
    }

    public class SimilarProduct
    {
        public Product Product { get; set; }
        public int SimilarityScore { get; set; }
        public string SelectionType { get; set; }  // "Similar" or "Random"
    }

    public interface IProductSimilarityService
    {
        Task<List<SimilarProduct>> GetFeaturedProducts(string productId, int count = 4);
    }

    public class ProductSimilarityService : IProductSimilarityService
    {
        private readonly IMongoCollection<Product> _products;
        private readonly Random _random;

        public ProductSimilarityService(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("Products");
            _random = new Random();
        }

        private string GetCategory(string description)
        {
            if (string.IsNullOrEmpty(description))
                return "Other Products";

            description = description.ToLower();
            if (description.Contains("sabun"))
                return "Soap Products";
            if (description.Contains("deterjan"))
                return "Cleaning Products";
            if (description.Contains("kazak"))
                return "Clothing";

            return "Other Products";
        }

        private HashSet<string> GetKeywords(string description)
        {
            if (string.IsNullOrEmpty(description))
                return new HashSet<string>();

            var stopWords = new HashSet<string> { "ve", "ile", "bir", "the", "in", "to", "of" };
            return new HashSet<string>(
                description.ToLower()
                    .Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(word => !stopWords.Contains(word))
            );
        }

        private async Task<List<Product>> GetRandomProducts(string productId, int count, HashSet<ObjectId> excludeIds)
        {
            var filter = Builders<Product>.Filter.Nin(p => p.Id, excludeIds);
            var products = await _products.Find(filter).ToListAsync();
            return products.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        public async Task<List<SimilarProduct>> GetFeaturedProducts(string productId, int count = 4)
        {
            var result = new List<SimilarProduct>();
            var excludeIds = new HashSet<ObjectId>();

            try
            {
                // Find reference product
                var referenceProduct = await _products.Find(p => p.Id == ObjectId.Parse(productId)).FirstOrDefaultAsync();
                if (referenceProduct == null)
                    return result;

                excludeIds.Add(referenceProduct.Id);

                // Get reference product properties
                var refKeywords = GetKeywords(referenceProduct.ProductDescription);
                var refCategory = GetCategory(referenceProduct.ProductDescription);

                // Find all other products
                var allProducts = await _products.Find(p => p.Id != referenceProduct.Id).ToListAsync();
                var similarProducts = new List<(Product Product, int Score)>();

                // Calculate similarity scores
                foreach (var product in allProducts)
                {
                    var productKeywords = GetKeywords(product.ProductDescription);
                    var productCategory = GetCategory(product.ProductDescription);

                    var commonKeywords = refKeywords.Intersect(productKeywords).Count();
                    var categoryMatch = refCategory == productCategory ? 2 : 0;
                    var similarityScore = commonKeywords + categoryMatch;

                    if (similarityScore > 0)
                    {
                        similarProducts.Add((product, similarityScore));
                    }
                }

                // Sort by similarity score and take top similar products
                var topSimilar = similarProducts
                    .OrderByDescending(x => x.Score)
                    .Take(count)
                    .ToList();

                // Add similar products
                foreach (var (product, score) in topSimilar)
                {
                    result.Add(new SimilarProduct
                    {
                        Product = product,
                        SimilarityScore = score,
                        SelectionType = "Similar"
                    });
                    excludeIds.Add(product.Id);
                }

                // Add random products if needed
                if (result.Count < count)
                {
                    var neededRandom = count - result.Count;
                    var randomProducts = await GetRandomProducts(productId, neededRandom, excludeIds);

                    foreach (var product in randomProducts)
                    {
                        result.Add(new SimilarProduct
                        {
                            Product = product,
                            SimilarityScore = 0,
                            SelectionType = "Random"
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetFeaturedProducts: {ex.Message}");
                return result;
            }
        }
    }
}