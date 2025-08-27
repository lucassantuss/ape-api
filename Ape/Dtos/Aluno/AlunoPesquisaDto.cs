namespace Ape.Dtos.Aluno
{
    public class AlunoPesquisaDto
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public string IdPersonal {  get; set; }
        public string NomePersonal { get; set; }
    }
}
