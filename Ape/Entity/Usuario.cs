using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ape.Entity
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
