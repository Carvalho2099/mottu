using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleManagement.Api.Models
{
    public class VehicleFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("vehicleId")]
        public string VehicleId { get; set; } = string.Empty;
        [BsonElement("fileName")]
        public string FileName { get; set; } = string.Empty;
        [BsonElement("fileUrl")]
        public string FileUrl { get; set; } = string.Empty;
        [BsonElement("uploadedAt")]
        public DateTime UploadedAt { get; set; }
    }
}
