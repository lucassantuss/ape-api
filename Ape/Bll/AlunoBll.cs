using Ape.Bll.Conversores;
using Ape.Dtos.Aluno;
using Ape.Dtos.Login;
using Ape.Entity;
using MongoDB.Driver;

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

        // Pesquisar Aluno por Usu�rio
        public List<AlunoPesquisaDto> PesquisarAlunoPorUsuario(string usuario)
        {
            try
            {
                return _database
                    .Find(f => f.Usuario.ToUpper() == usuario.ToUpper())
                    .Project(xs => new AlunoPesquisaDto
                    {
                        Id = xs.Id,
                        Usuario = xs.Usuario,
                        Nome = xs.Nome,
                        Email = xs.Email,
                        CPF = xs.CPF,                        
                        IdPersonal = xs.IdPersonal,
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar aluno por usu�rio: " + ex.Message, ex);
            }
        }

        // Pesquisar Aluno Por Id
        public AlunoPesquisaDto PesquisarAlunoPorId(string id)
        {
            try
            {
                return _database
                    .Find(f => f.Id == id)
                    .Project(xs => new AlunoPesquisaDto
                    {
                        Id = xs.Id,
                        Usuario = xs.Usuario,
                        Nome = xs.Nome,
                        Email = xs.Email,
                        CPF = xs.CPF,
                        IdPersonal = xs.IdPersonal,
                    })
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Pesquisa Aluno para Login (Usu�rio + Senha)
        public RetornoLoginDto PesquisarAlunoLogin(string usuario, string senha)
        {
            try
            {
                var aluno = _database
                    .Find(f => f.Usuario.ToUpper() == usuario.ToUpper())
                    .FirstOrDefault();

                if (aluno == null)
                    return null;

                // Verifica a senha criptografada com BCrypt
                bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, aluno.Senha);
                if (!senhaValida)
                    return null;

                return new RetornoLoginDto
                {
                    Id = aluno.Id,
                    Usuario = aluno.Usuario,
                };
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        // Pesquisar Aceite Personal Por Id Aluno
        public RetornoAcaoDto PesquisarAceitePersonalPorIdAluno(string id)
        {
            try
            {
                var alunoStatus = _database
                    .Find(f => f.Id == id)
                    .Project(xs => new RetornoAceitePersonalDto
                    {
                        AceitePersonal = xs.AceitePersonal,
                        DataAceitePersonal = xs.DataAceitePersonal,
                    })
                    .FirstOrDefault();

                if (alunoStatus == null)
                {
                    return new RetornoAcaoDto
                    {
                        Resultado = false,
                        Mensagem = "Aluno n�o encontrado."
                    };
                }

                if (string.IsNullOrEmpty(alunoStatus.DataAceitePersonal))
                {
                    return new RetornoAcaoDto
                    {
                        Resultado = false,
                        Mensagem = "O personal ainda n�o analisou seu v�nculo. Pendente de aprova��o."
                    };
                }
                else if (alunoStatus.AceitePersonal)
                {
                    return new RetornoAcaoDto
                    {
                        Resultado = true,
                        Mensagem = $"O personal aceitou seu v�nculo em {alunoStatus.DataAceitePersonal}."
                    };
                }
                else
                {
                    return new RetornoAcaoDto
                    {
                        Resultado = false,
                        Mensagem = $"O personal recusou seu v�nculo em {alunoStatus.DataAceitePersonal}."
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    // Senha criptografada
                    aluno.Senha = BCrypt.Net.BCrypt.HashPassword(alunoDto.Senha);

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
                    retorno.Mensagem = "Usu�rio j� cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (validaEmail)
                {
                    retorno.Mensagem = "Email j� cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }
                else if (validaCpf)
                {
                    retorno.Mensagem = "CPF j� cadastrado no sistema";
                    retorno.Resultado = false;
                    return retorno;
                }

                retorno.Mensagem = "Usu�rio v�lido";
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
                    retorno.Mensagem = "Aluno n�o encontrado.";
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

        // Alterar Aluno
        public RetornoAcaoDto AlterarAluno(string id, AlterarAlunoDto alunoDto)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                var alunoExistente = _database.Find(f => f.Id == id).FirstOrDefault();
                if (alunoExistente == null)
                {
                    retorno.Mensagem = "Aluno n�o encontrado para altera��o.";
                    retorno.Resultado = false;
                    return retorno;
                }

                // Verifica se o IdPersonal foi alterado
                bool personalAlterado = alunoExistente.IdPersonal != alunoDto.IdPersonal;

                var update = Builders<Aluno>.Update
                    .Set(a => a.Nome, alunoDto.Nome)
                    .Set(a => a.Usuario, alunoDto.Usuario)
                    .Set(a => a.Email, alunoDto.Email)
                    .Set(a => a.CPF, alunoDto.CPF)
                    .Set(a => a.IdPersonal, alunoDto.IdPersonal);

                // Se o personal mudou, resetar AceitePersonal e DataAceitePersonal
                if (personalAlterado)
                {
                    update = update
                        .Set(a => a.AceitePersonal, false)
                        .Set(a => a.DataAceitePersonal, "");
                }

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
                    retorno.Mensagem = "Aluno n�o encontrado para exclus�o.";
                    retorno.Resultado = false;
                    return retorno;
                }

                retorno.Mensagem = "Aluno exclu�do com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Remover v�nculo de Personal do Aluno
        public RetornoAcaoDto RemoverPersonal(string idAluno)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();

            try
            {
                var aluno = _database.Find(f => f.Id == idAluno).FirstOrDefault();
                if (aluno == null)
                {
                    retorno.Mensagem = "Aluno n�o encontrado.";
                    retorno.Resultado = false;
                    return retorno;
                }

                // Atualiza apenas o campo IdPersonal para null ou vazio
                var update = Builders<Aluno>.Update
                    .Set(a => a.IdPersonal, null)
                    .Set(a => a.AceitePersonal, false)
                    .Set(a => a.DataAceitePersonal, DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"));
                _database.UpdateOne(a => a.Id == aluno.Id, update);

                retorno.Mensagem = "Aluno desvinculado com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao remover v�nculo do personal: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        /// <summary>
        /// Aceita a solicita��o do aluno.
        /// </summary>
        public RetornoAcaoDto AceitarAluno(string idAluno)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                var update = Builders<Aluno>.Update
                    .Set(a => a.AceitePersonal, true)
                    .Set(a => a.DataAceitePersonal, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

                var result = _database.UpdateOne(a => a.Id == idAluno, update);

                if (result.ModifiedCount == 0)
                {
                    retorno.Resultado = false;
                    retorno.Mensagem = "Aluno n�o encontrado ou j� aceito.";
                }
                else
                {
                    retorno.Resultado = true;
                    retorno.Mensagem = "Aluno aceito com sucesso.";
                }
            }
            catch (Exception ex)
            {
                retorno.Resultado = false;
                retorno.Mensagem = $"Erro ao aceitar aluno: {ex.Message}";
            }

            return retorno;
        }

        /// <summary>
        /// Recusa a solicita��o do aluno.
        /// </summary>
        public RetornoAcaoDto RecusarAluno(string idAluno)
        {
            var retorno = new RetornoAcaoDto();
            try
            {
                var update = Builders<Aluno>.Update
                    .Set(a => a.AceitePersonal, false)
                    .Set(a => a.DataAceitePersonal, DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"));

                var result = _database.UpdateOne(a => a.Id == idAluno, update);

                if (result.ModifiedCount == 0)
                {
                    retorno.Resultado = false;
                    retorno.Mensagem = "Aluno n�o encontrado ou j� recusado.";
                }
                else
                {
                    retorno.Resultado = false;
                    retorno.Mensagem = "Aluno recusado com sucesso.";
                }
            }
            catch (Exception ex)
            {
                retorno.Resultado = false;
                retorno.Mensagem = $"Erro ao recusar aluno: {ex.Message}";
            }

            return retorno;
        }
    }
}