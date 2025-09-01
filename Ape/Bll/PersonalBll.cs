using Ape.Dtos.Aluno;
using Ape.Dtos.Login;
using Ape.Dtos.Personal;
using Ape.Entity;
using MongoDB.Driver;

namespace Ape.Bll
{
    public class PersonalBll
    {
        private readonly IMongoCollection<Personal> _database;

        public PersonalBll(IMongoCollection<Personal> database)
        {
            _database = database;
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

                // Senha criptografada
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

                _database.InsertOne(new Personal
                {
                    Nome = dto.Nome,
                    Usuario = dto.Usuario,
                    Email = dto.Email,
                    Senha = senhaHash,
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
                retorno.Mensagem = $"Erro ao criar personal: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        // Listar todos
        public List<PersonalSimplesDto> ListarPersonais()
        {
            return _database.AsQueryable()
                .Select(p => new PersonalSimplesDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                }).ToList();
        }

        // Pesquisa Personal para Login (Usuário + Senha)
        public RetornoLoginDto PesquisarPersonalLogin(string usuario, string senha)
        {
            try
            {
                var personal = _database
                    .Find(f => f.Usuario.ToUpper() == usuario.ToUpper())
                    .FirstOrDefault();

                if (personal == null)
                    return null;

                // Verifica a senha criptografada com BCrypt
                bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, personal.Senha);
                if (!senhaValida)
                    return null;

                return new RetornoLoginDto
                {
                    Id = personal.Id,
                    Usuario = personal.Usuario,
                };
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        // Pesquisar por usuário
        public PersonalDto PesquisarPersonalPorUsuario(string usuario)
        {
            var personal = _database.Find(f => f.Usuario == usuario).FirstOrDefault();
            return personal == null ? null : MapToDto(personal);
        }

        // Pesquisar por ID
        public PersonalDto PesquisarPersonalPorId(string id)
        {
            var personal = _database.Find(f => f.Id == id).FirstOrDefault();
            return personal == null ? null : MapToDto(personal);
        }

        // Pesquisar por aluno
        public PersonalDto PesquisarPersonalPorAluno(string idAluno, IMongoCollection<Aluno> alunosCollection)
        {
            var aluno = alunosCollection.Find(f => f.Id == idAluno).FirstOrDefault();
            if (aluno == null) return null;

            return PesquisarPersonalPorId(aluno.IdPersonal);
        }

        // Listar alunos do personal
        public List<AlunoSimplesDto> PesquisarAlunosDoPersonal(string idPersonal, IMongoCollection<Aluno> alunosCollection)
        {
            return alunosCollection.Find(f => f.IdPersonal == idPersonal)
                .Project(xs => new AlunoSimplesDto
                {
                    Id = xs.Id,
                    Nome = xs.Nome,
                    Email = xs.Email,
                }).ToList();
        }

        // Alterar Personal
        public RetornoAcaoDto AlterarPersonal(string id, AlterarPersonalDto dto)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                var personalExistente = _database.Find(f => f.Id == id).FirstOrDefault();
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
                    .Set(p => p.CPF, dto.CPF)
                    .Set(p => p.CREF, dto.CREF)
                    .Set(p => p.Estado, dto.Estado)
                    .Set(p => p.Cidade, dto.Cidade);

                _database.UpdateOne(p => p.Id == id, update);

                retorno.Mensagem = "Personal alterado com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao alterar personal: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
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
                retorno.Mensagem = $"Erro ao excluir personal: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        // Redefinir senha
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
                retorno.Mensagem = $"Erro ao redefinir senha: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        // Mapper auxiliar
        private PersonalDto MapToDto(Personal p)
        {
            return new PersonalDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Usuario = p.Usuario,
                Email = p.Email,
                Senha = p.Senha,
                CPF = p.CPF,
                CREF = p.CREF,
                Estado = p.Estado,
                Cidade = p.Cidade
            };
        }
    }
}
