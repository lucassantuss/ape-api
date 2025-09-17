using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ape.Entity
{
    public class Aluno
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("Usuario")]
        public string Usuario { get; set; }

        [BsonElement("Senha")]
        public string Senha { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("CPF")]
        public string CPF { get; set; }

        [BsonElement("Personal")]
        public string IdPersonal { get; set; }

        [BsonElement("AceitePersonal")]
        public bool AceitePersonal { get; set; }

        [BsonElement("DataAceitePersonal")]
        public DateTime? DataAceitePersonal { get; set; }

        [BsonElement("AceiteTermoLGPD")]
        public bool AceiteTermoLGPD { get; set; }

        [BsonElement("DataAceiteTermoLGPD")]
        public DateTime? DataAceiteTermoLGPD { get; set; }
    }
}