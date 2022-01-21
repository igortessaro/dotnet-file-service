using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Api.Entities
{
    public class File
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContenteBase64 { get; set; }
    }
}
