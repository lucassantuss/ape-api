using Ape.Dtos;
using Ape.Entity;

namespace Ape.Dtos
{
    public class AlunoDto
    {
        //public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public int IdPersonal { get; set; }
    }
}