using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ape.Entity
{
    public class Exercicio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Data")]
        public string Data { get; set; }

        [BsonElement("Repeticoes")]
        public string Repeticoes { get; set; }

        [BsonElement("ObservacoesAluno")]
        public string ObservacoesAluno { get; set; }

        [BsonElement("ObservacoesPersonal")]
        public string ObservacoesPersonal { get; set; }

        [BsonElement("Aluno")]
        public Aluno Aluno { get; set; }
    }
}