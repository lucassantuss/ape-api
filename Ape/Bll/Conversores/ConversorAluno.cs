using Ape.Dtos.Aluno;
using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorAluno
    {
        public Aluno ConverterAlunoDto(AlunoDto dto)
        {
            Aluno entidade = new Aluno();
            //entidade.Id = dto.Id.ToString();
            entidade.Usuario = dto.Usuario;
            entidade.Nome = dto.Nome;
            entidade.Usuario = dto.Usuario;
            entidade.Email = dto.Email;
            entidade.CPF = dto.CPF;
            entidade.Senha = dto.Senha;
            entidade.IdPersonal = dto.IdPersonal.ToString();
            entidade.AceiteTermoLGPD = true;
            entidade.DataAceiteTermoLGPD = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
            
            return entidade;
        }
    }
}