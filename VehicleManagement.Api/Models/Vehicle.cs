using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleManagement.Api.Models
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("model")]
        public string Model { get; set; } = string.Empty;
        [BsonElement("year")]
        public int Year { get; set; }
        [BsonElement("plate")]
        public string Plate { get; set; } = string.Empty;
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
