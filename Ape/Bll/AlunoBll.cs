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

        //private Aluno ConverterAlunoDto(AlunoDto dto)
        //{
        //    Aluno entidade = new Aluno();
        //    //entidade.Id = dto.Id.ToString();
        //    entidade.Usuario = dto.Usuario;
        //    entidade.Nome = dto.Nome;
        //    entidade.Usuario = dto.Usuario;
        //    entidade.Email = dto.Email;
        //    entidade.CPF = dto.CPF;
        //    entidade.Senha = dto.Senha;
        //    entidade.IdPersonal = dto.IdPersonal.ToString();
        //    return entidade;
        //}

        //private AlunoDto ConverterAluno(Aluno entidade)
        //{
        //    AlunoDto dto = new AlunoDto();
        //    //dto.Id = int.Parse(entidade.Id);
        //    dto.Usuario = entidade.Usuario;
        //    dto.Nome = entidade.Nome;
        //    dto.Usuario = entidade.Usuario;
        //    dto.Email = entidade.Email;
        //    dto.CPF = entidade.CPF;
        //    dto.Senha = entidade.Senha;
        //    dto.IdPersonal = int.Parse(entidade.IdPersonal);
        //    return dto;
        //}

        private RetornoAcaoDto ValidarCadastro(Aluno aluno)
        {
            try
            {
                RetornoAcaoDto retorno = new RetornoAcaoDto();

                bool validaUsuario = database.Find(f => f.Usuario == aluno.Usuario) != null;
                bool validaEmail = database.Find(f => f.Email == aluno.Email) != null;
                bool validaCpf = database.Find(f => f.CPF == aluno.CPF) != null;

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

        // TODO Métodos para criar

        // Redefinir Senha Aluno
        // Pesquisar Aluno Por Usuario
        // Pesquisar Aluno Por Id
        // Alterar Aluno
        // Excluir Aluno
    }
}