using MongoDB.Driver;
using Ape.Entity;
using Ape.Dtos;
using Ape.Bll.Conversores;

namespace Ape.Bll
{
    public class AlunoBll
    {
        private readonly IMongoCollection<Aluno> _database;
        private readonly HttpClient _client;

        public AlunoBll(IMongoCollection<Aluno> database)
        {
            _database = database;
            _client = new HttpClient();
        }

        // Pesquisar Aluno
        public List<Aluno> PesquisarAluno(LoginDto alunoDto)
        {
            try
            {
                List<Aluno> aluno = new List<Aluno>();
                aluno = _database
                    .Find(f => f.Usuario.ToUpper() == alunoDto.Usuario.ToUpper())
                    .ToList();

                return aluno;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        // Criar Aluno
        public RetornoAcaoDto CriarAluno(AlunoDto alunoDto)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();

            try
            {
                Aluno aluno = new ConversorAluno().ConverterAlunoDto(alunoDto);
                RetornoAcaoDto validaCadastro = ValidarCadastro(aluno);

                if (validaCadastro.Resultado)
                {
                    _database.InsertOne(aluno);
                }
                retorno.Mensagem = validaCadastro.Mensagem;
                retorno.Resultado = validaCadastro.Resultado;
                return retorno;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        // Validar Cadastro
        private RetornoAcaoDto ValidarCadastro(Aluno aluno)
        {
            try
            {
                RetornoAcaoDto retorno = new RetornoAcaoDto();

                bool validaUsuario = _database.Find(f => f.Usuario == aluno.Usuario).Any();
                bool validaEmail = _database.Find(f => f.Email == aluno.Email).Any();
                bool validaCpf = _database.Find(f => f.CPF == aluno.CPF).Any();

                if (validaUsuario)
                {
                    retorno.Mensagem = "Usuário já cadastrado no sistema";
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

                retorno.Mensagem = "Usuário válido";
                retorno.Resultado = true;

                return retorno;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        // Redefinir Senha Aluno
        public RetornoAcaoDto RedefinirSenha(string usuario, string novaSenha)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                var aluno = _database.Find(f => f.Usuario == usuario).FirstOrDefault();
                if (aluno == null)
                {
                    retorno.Mensagem = "Aluno não encontrado.";
                    retorno.Resultado = false;
                    return retorno;
                }

                var update = Builders<Aluno>.Update.Set(a => a.Senha, novaSenha);
                _database.UpdateOne(a => a.Id == aluno.Id, update);

                retorno.Mensagem = "Senha redefinida com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Pesquisar Aluno Por Usuario
        public AlunoDto PesquisarAlunoPorUsuario(string usuario)
        {
            try
            {
                var aluno = _database.Find(f => f.Usuario == usuario).FirstOrDefault();
                if (aluno == null)
                    return null;

                return new ConversorAluno().ConverterAluno(aluno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Pesquisar Aluno Por Id
        public AlunoDto PesquisarAlunoPorId(string id)
        {
            try
            {
                var aluno = _database.Find(f => f.Id == id).FirstOrDefault();
                if (aluno == null)
                    return null;

                return new ConversorAluno().ConverterAluno(aluno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Alterar Aluno
        public RetornoAcaoDto AlterarAluno(string id, AlunoDto alunoDto)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                var alunoExistente = _database.Find(f => f.Id == id).FirstOrDefault();
                if (alunoExistente == null)
                {
                    retorno.Mensagem = "Aluno não encontrado para alteração.";
                    retorno.Resultado = false;
                    return retorno;
                }

                var update = Builders<Aluno>.Update
                    .Set(a => a.Nome, alunoDto.Nome)
                    .Set(a => a.Usuario, alunoDto.Usuario)
                    .Set(a => a.Email, alunoDto.Email)
                    .Set(a => a.CPF, alunoDto.CPF)
                    .Set(a => a.Senha, alunoDto.Senha)
                    .Set(a => a.IdPersonal, alunoDto.IdPersonal.ToString());

                _database.UpdateOne(a => a.Id == alunoExistente.Id, update);

                retorno.Mensagem = "Aluno alterado com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao alterar aluno: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        // Excluir Aluno
        public RetornoAcaoDto ExcluirAluno(string id)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                var result = _database.DeleteOne(f => f.Id == id);
                if (result.DeletedCount == 0)
                {
                    retorno.Mensagem = "Aluno não encontrado para exclusão.";
                    retorno.Resultado = false;
                    return retorno;
                }

                retorno.Mensagem = "Aluno excluído com sucesso.";
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