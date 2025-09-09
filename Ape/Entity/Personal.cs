using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ape.Entity
{
    public class Personal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("Usuario")]
        public string Usuario { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Senha")]
        public string Senha { get; set; }

        [BsonElement("CPF")]
        public string CPF { get; set; }

        [BsonElement("NumeroCREF")]
        public string NumeroCREF { get; set; }

        [BsonElement("CategoriaCREF")]
        public string CategoriaCREF { get; set; }

        [BsonElement("SiglaCREF")]
        public string SiglaCREF { get; set; }

        [BsonElement("Estado")]
        public string Estado { get; set; }

        [BsonElement("Cidade")]
        public string Cidade { get; set; }

        [BsonElement("AceiteTermoLGPD")]
        public bool AceiteTermoLGPD { get; set; }

        [BsonElement("DataAceiteTermoLGPD")]
        public string DataAceiteTermoLGPD { get; set; }
    }
}