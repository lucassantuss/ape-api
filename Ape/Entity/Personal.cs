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

        [BsonElement("Estado")]
        public string Estado { get; set; }

        [BsonElement("Cidade")]
        public string Cidade { get; set; }

        [BsonElement("NumeroCref")]
        public string NumeroCref { get; set; }

        [BsonElement("CategoriaCref")]
        public string CategoriaCref { get; set; }

        [BsonElement("SiglaCref")]
        public string SiglaCref { get; set; }
              
        [BsonElement("AceiteTermoLGPD")]
        public bool AceiteTermoLGPD { get; set; }

        [BsonElement("DataAceiteTermoLGPD")]
        public DateTime? DataAceiteTermoLGPD { get; set; }
    }
}