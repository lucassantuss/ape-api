namespace Ape.Dtos.Personal
{
    public class PersonalPesquisaDto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public string NumeroCref { get; set; }
        public string CategoriaCref { get; set; }
        public string SiglaCref { get; set; }
    }
}