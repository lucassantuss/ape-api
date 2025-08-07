using Ape.Dtos;
using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorAluno
    {
        public Aluno ConverterAlunoDto(AlunoDto dto)
        {
            Aluno entidade = new Aluno();

            entidade.Id = dto.Id;
            entidade.Usuario = dto.Usuario;
            entidade.Nome = dto.Nome;
            entidade.Usuario = dto.Usuario;
            entidade.Email = dto.Email;
            entidade.CPF = dto.CPF;
            entidade.Senha = dto.Senha;
            entidade.Personal = new Personal
            {
                Id = dto.Personal.Id,
                Nome = dto.Personal.Nome,
                Email = dto.Personal.Email,
            };

            return entidade;
        }

        public AlunoDto ConverterAluno(Aluno entidade)
        {
            AlunoDto dto = new AlunoDto();
            dto.Id = entidade.Id;
            dto.Usuario = entidade.Usuario;
            dto.Nome = entidade.Nome;
            dto.Usuario = entidade.Usuario;
            dto.Email = entidade.Email;
            dto.CPF = entidade.CPF;
            dto.Senha = entidade.Senha;
            dto.Personal = new PersonalDto
            {
                Id = entidade.Personal.Id,
                Nome = entidade.Personal.Nome,
                Email = entidade.Personal.Email,
            };

            return dto;
        }
    }
}