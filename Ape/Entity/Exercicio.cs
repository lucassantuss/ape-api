using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ape.Entity
{
    public class Exercicio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("DataExecucao")]
        public DateTime? DataExecucao { get; set; }

        [BsonElement("QuantidadeRepeticoes")]
        public string QuantidadeRepeticoes { get; set; }

        [BsonElement("PorcentagemAcertos")]
        public string PorcentagemAcertos { get; set; }

        [BsonElement("TempoExecutado")]
        public string TempoExecutado { get; set; }

        [BsonElement("ObservacoesAluno")]
        public string ObservacoesAluno { get; set; }

        [BsonElement("ObservacoesPersonal")]
        public string ObservacoesPersonal { get; set; }

        [BsonElement("Aluno")]
        public string IdAluno { get; set; }
    }
}