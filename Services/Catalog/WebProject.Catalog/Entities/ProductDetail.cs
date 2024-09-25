using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebProject.Catalog.Entities
{
    public class ProductDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductDetailId { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductInfo { get; set; }

        [BsonIgnore]
        public Product Product { get; set; }
    }
}
