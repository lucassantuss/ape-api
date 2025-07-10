using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ape.Entity
{
    public class Aluno
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
        public Personal Personal { get; set; }
    }
}
