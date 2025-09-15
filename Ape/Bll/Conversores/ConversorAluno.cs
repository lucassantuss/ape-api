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
            entidade.DataAceitePersonal = "";

            entidade.AceiteTermoLGPD = true;
            entidade.DataAceiteTermoLGPD = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss");
            
            return entidade;
        }
    }
}