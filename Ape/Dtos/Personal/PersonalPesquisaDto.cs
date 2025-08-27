namespace Ape.Dtos.Personal
{
    public class PersonalPesquisaDto
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public string CategoriaProfissional { get; set; }
        public string CREF { get; set; }
    }
}