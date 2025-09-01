using Ape.Bll.Conversores;
using Ape.Dtos.Aluno;
using Ape.Dtos.Exercicio;
using Ape.Entity;
using MongoDB.Driver;

namespace Ape.Bll
{
    /// <summary>
    /// Classe de regras de neg�cio para a entidade Exercicio.
    /// Respons�vel por interagir com o banco MongoDB e aplicar regras antes do Controller.
    /// </summary>
    public class ExercicioBll
    {
        private readonly IMongoCollection<Exercicio> _database;

        public ExercicioBll(IMongoCollection<Exercicio> database)
        {
            _database = database;
        }

        /// <summary>
        /// Salva um novo resultado de exerc�cio no banco.
        /// </summary>
        public RetornoAcaoDto SalvarResultados(ExercicioDto exercicioDto)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();

            try
            {
                Exercicio exercicio = new ConversorExercicio().ConverterExercicioDto(exercicioDto);

                _database.InsertOne(exercicio);
                retorno.Mensagem = "Resultado salvo com sucesso.";
                retorno.Resultado = true;
                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao salvar resultado: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        /// <summary>
        /// Pesquisa exerc�cio pelo Id do exerc�cio.
        /// </summary>
        public Exercicio PesquisarPorId(string idExercicio)
        {
            try
            {
                return _database
                    .Find(f => f.Id == idExercicio)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao pesquisar exerc�cio: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os exerc�cios de um aluno espec�fico.
        /// </summary>
        public List<Exercicio> ListarPorIdUser(string idUser)
        {
            try
            {
                return _database
                    .Find(f => f.IdAluno == idUser)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar exerc�cios do aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Adiciona uma observa��o do aluno a um exerc�cio.
        /// </summary>
        public RetornoAcaoDto AdicionarObservacaoAluno(string idExercicio, string observacao)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                var update = Builders<Exercicio>.Update.Set(e => e.ObservacoesAluno, observacao);
                var result = _database.UpdateOne(e => e.Id == idExercicio, update);

                if (result.ModifiedCount == 0)
                {
                    retorno.Mensagem = "Exerc�cio n�o encontrado para adicionar observa��o do aluno.";
                    retorno.Resultado = false;
                }
                else
                {
                    retorno.Mensagem = "Observa��o do aluno adicionada com sucesso.";
                    retorno.Resultado = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao adicionar observa��o do aluno: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        /// <summary>
        /// Adiciona uma observa��o do personal a um exerc�cio.
        /// </summary>
        public RetornoAcaoDto AdicionarObservacaoPersonal(string idExercicio, string observacao)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            try
            {
                var update = Builders<Exercicio>.Update.Set(e => e.ObservacoesPersonal, observacao);
                var result = _database.UpdateOne(e => e.Id == idExercicio, update);

                if (result.ModifiedCount == 0)
                {
                    retorno.Mensagem = "Exerc�cio n�o encontrado para adicionar observa��o do personal.";
                    retorno.Resultado = false;
                }
                else
                {
                    retorno.Mensagem = "Observa��o do personal adicionada com sucesso.";
                    retorno.Resultado = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao adicionar observa��o do personal: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }
    }
}
