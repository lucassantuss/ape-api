using System;
using MongoDB.Driver;
using Ape.Entity;
using Ape.Dtos;

namespace Ape.Bll
{
    public class AlunoBll
    {
        private readonly IMongoCollection<Aluno> database;
        private readonly HttpClient client;

        public AlunoBll(IMongoCollection<Aluno> _database)
        {
            database = _database;
            client = new HttpClient();
        }

        public Aluno PesquisarAluno(AlunoDto alunoDto)
        {
            try
            {
                Aluno aluno = new Aluno();
                aluno = database.Find(f => f.Usuario == alunoDto.Usuario).FirstOrDefault();
                return aluno;
            }
            catch(Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        public RetornoAcaoDto ValidarLogin(AlunoDto alunoDto)
        {
            try
            {
                RetornoAcaoDto retorno = new RetornoAcaoDto();
                Aluno aluno = database.Find(f => f.Usuario == alunoDto.Usuario && f.Senha == alunoDto.Senha).FirstOrDefault();
                if(aluno != null)
                {
                    retorno.Mensagem = "Acesso autorizado!";
                    retorno.Sucesso = true;
                }
                else
                {
                    retorno.Mensagem = "Acesso não autorizado!";
                    retorno.Sucesso = false;
                }
                return retorno;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        public RetornoAcaoDto CriarAluno(AlunoDto alunoDto)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                Aluno aluno = database.Find(u => u.Usuario == alunoDto.Usuario && u.Email == alunoDto.Email).FirstOrDefault();
                if (aluno == null)
                {
                    database.InsertOne(aluno);
                    retorno.Mensagem = "Aluno criado com sucesso";
                    retorno.Sucesso = true;
                }
                else
                {
                    retorno.Mensagem = "Aluno já existente";
                    retorno.Sucesso = false;
                }
                return retorno;
            }
            catch
            {
                throw new ArgumentException("Não foi possível criar o aluno");
            }

        }

        private Aluno ConverterAlunoDto(AlunoDto dto)
        {
            Aluno entidade = new Aluno();
            entidade.Id = dto.Id;
            entidade.Usuario = dto.Usuario;
            entidade.Nome = dto.Nome;
            entidade.Usuario = dto.Usuario;
            entidade.Email = dto.Email;
            entidade.Senha = dto.Senha;
            entidade.Personal = dto.Personal;
            return entidade;
        }

        private AlunoDto ConverterAluno(Aluno entidade)
        {
            AlunoDto dto = new AlunoDto();
            dto.Id = entidade.Id;
            dto.Usuario = entidade.Usuario;
            dto.Nome = entidade.Nome;
            dto.Usuario = entidade.Usuario;
            dto.Email = entidade.Email;
            dto.Senha = entidade.Senha;
            dto.Personal = entidade.Personal;
            return dto;
        }
    }
}