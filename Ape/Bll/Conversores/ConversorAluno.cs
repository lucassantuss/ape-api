using Ape.Dtos.Aluno;
using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorAluno
    {
        public Aluno ConverterAlunoDto(AlunoDto dto)
        {
            Aluno entidade = new Aluno();

            entidade.Usuario = dto.Usuario;
            entidade.Nome = dto.Nome;
            entidade.Email = dto.Email;
            entidade.CPF = dto.CPF;
            entidade.Senha = dto.Senha;
            entidade.IdPersonal = dto.IdPersonal.ToString();

            entidade.AceitePersonal = false;
            entidade.DataAceitePersonal = null;

            entidade.AceiteTermoLGPD = dto.AceiteTermos;
            entidade.DataAceiteTermoLGPD = DateTime.UtcNow;
            
            return entidade;
        }
    }
}