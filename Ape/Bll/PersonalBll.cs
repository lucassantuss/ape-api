using Ape.Entity;
using MongoDB.Driver;

namespace Ape.Bll
{
    public class PersonalBll
    {
        private readonly IMongoCollection<Personal> _database;
        private readonly HttpClient _client;

        public PersonalBll(IMongoCollection<Personal> database)
        {
            _database = database;
            _client = new HttpClient();
        }

        // Pesquisar Personal
        public Personal PesquisarPersonal(PersonalDto personalDto)
        {
            try
            {
                return _database.Find(f => f.Usuario == personalDto.Usuario).FirstOrDefault();
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        // Criar Personal
        public RetornoAcaoDto CriarPersonal(PersonalDto dto)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                if (_database.Find(f => f.Usuario == dto.Usuario).FirstOrDefault() != null)
                {
                    retorno.Mensagem = "Usuário já cadastrado.";
                    retorno.Resultado = false;
                    return retorno;
                }

                _database.InsertOne(new Personal
                {
                    Nome = dto.Nome,
                    Usuario = dto.Usuario,
                    Email = dto.Email,
                    Senha = dto.Senha,
                    CPF = dto.CPF,
                    CREF = dto.CREF,
                    Estado = dto.Estado,
                    Cidade = dto.Cidade
                });

                retorno.Mensagem = "Personal criado com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Redefinir Senha Personal
        public RetornoAcaoDto RedefinirSenha(string usuario, string novaSenha)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                var personal = _database.Find(f => f.Usuario == usuario).FirstOrDefault();
                if (personal == null)
                {
                    retorno.Mensagem = "Personal não encontrado.";
                    retorno.Resultado = false;
                    return retorno;
                }

                var update = Builders<Personal>.Update.Set(p => p.Senha, novaSenha);
                _database.UpdateOne(p => p.Id == personal.Id, update);

                retorno.Mensagem = "Senha redefinida com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Pesquisar Personal Por Usuario
        public PersonalDto PesquisarPersonalPorUsuario(string usuario)
        {
            var personal = _database.Find(f => f.Usuario == usuario).FirstOrDefault();
            if (personal == null) return null;

            return new PersonalDto
            {
                Id = personal.Id,
                Nome = personal.Nome,
                Usuario = personal.Usuario,
                Email = personal.Email,
                Senha = personal.Senha,
                CPF = personal.CPF,
                CREF = personal.CREF,
                Estado = personal.Estado,
                Cidade = personal.Cidade
            };
        }

        // Pesquisar Personal Por Id
        public PersonalDto PesquisarPersonalPorId(string id)
        {
            var personal = _database.Find(f => f.Id == id).FirstOrDefault();
            if (personal == null) return null;

            return new PersonalDto
            {
                Id = personal.Id,
                Nome = personal.Nome,
                Usuario = personal.Usuario,
                Email = personal.Email,
                Senha = personal.Senha,
                CPF = personal.CPF,
                CREF = personal.CREF,
                Estado = personal.Estado,
                Cidade = personal.Cidade
            };
        }

        // Pesquisar Personal Por Aluno
        public PersonalDto PesquisarPersonalPorAluno(string idAluno, IMongoCollection<Aluno> alunosCollection)
        {
            var aluno = alunosCollection.Find(f => f.Id == idAluno).FirstOrDefault();
            if (aluno == null) return null;

            return PesquisarPersonalPorId(aluno.IdPersonal);
        }

        // Pesquisar Alunos do Personal
        public List<Aluno> PesquisarAlunosDoPersonal(string idPersonal, IMongoCollection<Aluno> alunosCollection)
        {
            return alunosCollection.Find(f => f.IdPersonal == idPersonal).ToList();
        }

        // Alterar Personal
        public RetornoAcaoDto AlterarPersonal(PersonalDto dto)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                var personalExistente = _database.Find(f => f.Id == dto.Id).FirstOrDefault();
                if (personalExistente == null)
                {
                    retorno.Mensagem = "Personal não encontrado.";
                    retorno.Resultado = false;
                    return retorno;
                }

                var update = Builders<Personal>.Update
                    .Set(p => p.Nome, dto.Nome)
                    .Set(p => p.Usuario, dto.Usuario)
                    .Set(p => p.Email, dto.Email)
                    .Set(p => p.Senha, dto.Senha)
                    .Set(p => p.CPF, dto.CPF)
                    .Set(p => p.CREF, dto.CREF)
                    .Set(p => p.Estado, dto.Estado)
                    .Set(p => p.Cidade, dto.Cidade);

                _database.UpdateOne(p => p.Id == dto.Id, update);

                retorno.Mensagem = "Personal alterado com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Excluir Personal
        public RetornoAcaoDto ExcluirPersonal(string id)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                var result = _database.DeleteOne(f => f.Id == id);
                if (result.DeletedCount == 0)
                {
                    retorno.Mensagem = "Personal não encontrado.";
                    retorno.Resultado = false;
                    return retorno;
                }

                retorno.Mensagem = "Personal excluído com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}