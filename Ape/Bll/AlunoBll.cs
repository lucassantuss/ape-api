using MongoDB.Driver;
using Ape.Entity;
using Ape.Dtos;
using Ape.Bll.Conversores;

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

        public List<Aluno> PesquisarAluno(AlunoDto alunoDto)
        {
            try
            {
                List<Aluno> aluno = new List<Aluno>();
                aluno = database.Find(f => f.Usuario == alunoDto.Usuario).ToList();

                return aluno;
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
                Aluno aluno = new ConversorAluno().ConverterAlunoDto(alunoDto);
                RetornoAcaoDto validaCadastro = ValidarCadastro(aluno);

                if (validaCadastro.Resultado)
                {
                    database.InsertOne(aluno);
                    retorno.Mensagem = "Aluno criado com sucesso";
                    retorno.Resultado = true;
                }

                return retorno;
            }
            catch
            {
                throw new ArgumentException("Não foi possível criar o aluno");
            }
        }

        public RetornoAcaoDto ValidarLogin(AlunoDto alunoDto)
        {
            try
            {
                RetornoAcaoDto retorno = new RetornoAcaoDto();

                Aluno aluno = database.Find(f => (f.Usuario == alunoDto.Usuario || 
                                                  f.Email == alunoDto.Email) && 
                                                  f.Senha == alunoDto.Senha).FirstOrDefault();

                if (aluno != null)
                {
                    retorno.Mensagem = "Acesso autorizado!";
                    retorno.Resultado = true;
                }
                else
                {
                    retorno.Mensagem = "Acesso não autorizado!";
                    retorno.Resultado = false;
                }

                return retorno;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        private RetornoAcaoDto ValidarCadastro(Aluno aluno)
        {
            try
            {
                RetornoAcaoDto retorno = new RetornoAcaoDto();

                bool validaUsuario = database.Find(f => f.Usuario == aluno.Usuario) != null;
                bool validaEmail = database.Find(f => f.Email == aluno.Email) != null;
                bool validaCpf = database.Find(f => f.CPF == aluno.CPF) != null;

                if (!validaUsuario)
                {
                    retorno.Mensagem = "Usuário já cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (!validaEmail)
                {
                    retorno.Mensagem = "Email já cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (!validaCpf)
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

        // TODO Métodos para criar

        // Redefinir Senha Aluno
        // Pesquisar Aluno Por Usuario
        // Pesquisar Aluno Por Id
        // Alterar Aluno
        // Excluir Aluno
    }
}