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
            return entidade;
        }

        public AlunoDto ConverterAluno(Aluno entidade)
        {
            AlunoDto dto = new AlunoDto();
            //dto.Id = int.Parse(entidade.Id);
            dto.Usuario = entidade.Usuario;
            dto.Nome = entidade.Nome;
            dto.Usuario = entidade.Usuario;
            dto.Email = entidade.Email;
            dto.CPF = entidade.CPF;
            dto.Senha = entidade.Senha;
            dto.IdPersonal = entidade.IdPersonal;
            return dto;
        }
    }
}