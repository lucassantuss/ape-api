using Ape.Bll.Conversores;
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
        public RetornoAcaoDto CriarPersonal(PersonalDto personalDto)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                Personal personal = new ConversorPersonal().ConverterPersonalDto(personalDto);
                RetornoAcaoDto validaCadastro = ValidarCadastro(personal);

                if (validaCadastro.Resultado)
                {
                    // Senha criptografada
                    personal.Senha = BCrypt.Net.BCrypt.HashPassword(personalDto.Senha);

                    _database.InsertOne(personal);
                }

                retorno.Mensagem = validaCadastro.Mensagem;
                retorno.Resultado = validaCadastro.Resultado;
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
        public PersonalPesquisaDto PesquisarPersonalPorUsuario(string usuario)
        {
            var personal = _database.Find(f => f.Usuario == usuario).FirstOrDefault();
            return personal == null ? null : MapToDto(personal);
        }

        // Pesquisar por ID
        public PersonalPesquisaDto PesquisarPersonalPorId(string id)
        {
            var personal = _database.Find(f => f.Id == id).FirstOrDefault();
            return personal == null ? null : MapToDto(personal);
        }

        // Pesquisar por aluno
        public PersonalPesquisaDto PesquisarPersonalPorAluno(string idAluno, IMongoCollection<Aluno> alunosCollection)
        {
            var aluno = alunosCollection.Find(f => f.Id == idAluno).FirstOrDefault();
            if (aluno == null) return null;

            return PesquisarPersonalPorId(aluno.IdPersonal);
        }

        // Listar alunos do personal
        public List<AlunoSimplesDto> PesquisarAlunosDoPersonal(string idPersonal, IMongoCollection<Aluno> alunosCollection)
        {
            var alunos = alunosCollection
                .Find(f => f.IdPersonal == idPersonal)
                .SortByDescending(f => f.Nome)
                .ToList();

            var brasiliaTZ = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            return alunos
                .Select(xs => new AlunoSimplesDto
                {
                    Id = xs.Id,
                    Usuario = xs.Usuario,
                    Nome = xs.Nome,
                    Email = xs.Email,
                    AceitePersonal = xs.AceitePersonal,
                    DataAceitePersonal = xs.DataAceitePersonal.HasValue
                        ? TimeZoneInfo.ConvertTimeFromUtc(xs.DataAceitePersonal.Value, brasiliaTZ)
                            .ToString("dd/MM/yyyy - HH:mm:ss")
                        : ""
                })
                .ToList();
        }

        // Validar Cadastro
        private RetornoAcaoDto ValidarCadastro(Personal personal)
        {
            try
            {
                RetornoAcaoDto retorno = new RetornoAcaoDto();

                bool validaUsuario = _database.Find(f => f.Usuario == personal.Usuario).Any();
                bool validaEmail = _database.Find(f => f.Email == personal.Email).Any();
                bool validaCpf = _database.Find(f => f.CPF == personal.CPF).Any();
                bool validaCref = _database.Find(f => f.NumeroCref == personal.NumeroCref &
                                                      f.CategoriaCref == personal.CategoriaCref &
                                                      f.SiglaCref == personal.SiglaCref
                                                ).Any();

                if (validaUsuario)
                {
                    retorno.Mensagem = "Nome de usuário já cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (validaEmail)
                {
                    retorno.Mensagem = "Email já cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (validaCpf)
                {
                    retorno.Mensagem = "CPF já cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (validaCref)
                {
                    retorno.Mensagem = "CREF já cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }

                if (personal.AceiteTermoLGPD == false)
                {
                    retorno.Mensagem = "É necessário aceitar os Termos de Uso e Política de Privacidade.";
                    retorno.Resultado = false;
                    return retorno;
                }

                retorno.Mensagem = "Personal criado com sucesso.";
                retorno.Resultado = true;

                return retorno;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
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
                    .Set(p => p.Estado, dto.Estado)
                    .Set(p => p.Cidade, dto.Cidade)
                    .Set(p => p.NumeroCref, dto.NumeroCref)
                    .Set(p => p.CategoriaCref, dto.CategoriaCref)
                    .Set(p => p.SiglaCref, dto.SiglaCref);

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
        private PersonalPesquisaDto MapToDto(Personal p)
        {
            return new PersonalPesquisaDto
            {
                Nome = p.Nome,
                Usuario = p.Usuario,
                Email = p.Email,
                CPF = p.CPF,
                Estado = p.Estado,
                Cidade = p.Cidade,

                NumeroCref = p.NumeroCref,
                CategoriaCref = p.CategoriaCref,
                SiglaCref = p.SiglaCref,
            };
        }
    }
}
