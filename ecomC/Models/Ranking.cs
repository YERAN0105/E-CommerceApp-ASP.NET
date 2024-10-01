using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ecomC.Models
{
    public class Ranking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RankingId { get; set; }
        
        public string VendorId { get; set; }
        
        public string CustomerId { get; set; }
        
        public int Rating { get; set; } // Ranking value (e.g., 1 to 5)
        
        public string Comment { get; set; } // Customer comment
    }
}