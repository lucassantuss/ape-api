using Ape.Bll.Conversores;
using Ape.Dtos.Aluno;
using Ape.Dtos.Exercicio;
using Ape.Entity;
using MongoDB.Driver;

namespace Ape.Bll
{
    /// <summary>
    /// Classe de regras de negócio para a entidade Exercicio.
    /// Responsável por interagir com o banco MongoDB e aplicar regras antes do Controller.
    /// </summary>
    public class ExercicioBll
    {
        private readonly IMongoCollection<Exercicio> _database;

        public ExercicioBll(IMongoCollection<Exercicio> database)
        {
            _database = database;
        }

        /// <summary>
        /// Salva um novo resultado de exercício no banco.
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
        /// Pesquisa exercício pelo Id do exercício.
        /// </summary>
        public ExercicioPesquisaDto PesquisarPorId(string idExercicio)
        {
            try
            {
                var exercicio = _database
                    .Find(f => f.Id == idExercicio)
                    .FirstOrDefault();

                if (exercicio == null)
                    return null;

                string dataExecucaoFormatada = exercicio.DataExecucao.HasValue
                    ? TimeZoneInfo.ConvertTimeFromUtc(
                        exercicio.DataExecucao.Value,
                        TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
                    ).ToString("dd/MM/yyyy - HH:mm:ss")
                    : "";

                return new ExercicioPesquisaDto
                {
                    Id = exercicio.Id,
                    Nome = exercicio.Nome,
                    DataExecucao = dataExecucaoFormatada,
                    QuantidadeRepeticoes = exercicio.QuantidadeRepeticoes,
                    PorcentagemAcertos = exercicio.PorcentagemAcertos,
                    TempoExecutado = exercicio.TempoExecutado,
                    ObservacoesAluno = exercicio.ObservacoesAluno,
                    ObservacoesPersonal = exercicio.ObservacoesPersonal,
                    IdAluno = exercicio.IdAluno,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao pesquisar exercício: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os exercícios de um aluno específico.
        /// </summary>
        public List<ExercicioPesquisaDto> ListarPorIdUser(string idUser)
        {
            try
            {
                var exercicios = _database
                    .Find(f => f.IdAluno == idUser)
                    .SortByDescending(f => f.DataExecucao)
                    .ToList();

                var brasiliaTZ = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

                return exercicios.Select(xs => new ExercicioPesquisaDto
                {
                    Id = xs.Id,
                    Nome = xs.Nome,
                    DataExecucao = xs.DataExecucao.HasValue
                        ? TimeZoneInfo.ConvertTimeFromUtc(xs.DataExecucao.Value, brasiliaTZ)
                            .ToString("dd/MM/yyyy - HH:mm:ss")
                        : "",
                    QuantidadeRepeticoes = xs.QuantidadeRepeticoes,
                    PorcentagemAcertos = xs.PorcentagemAcertos,
                    TempoExecutado = xs.TempoExecutado,
                    ObservacoesAluno = xs.ObservacoesAluno,
                    ObservacoesPersonal = xs.ObservacoesPersonal,
                    IdAluno = xs.IdAluno,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar exercícios do aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Adiciona uma observação do aluno a um exercício.
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
                    retorno.Mensagem = "Exercício não encontrado para adicionar observação do aluno.";
                    retorno.Resultado = false;
                }
                else
                {
                    retorno.Mensagem = "Observação do aluno adicionada com sucesso.";
                    retorno.Resultado = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao adicionar observação do aluno: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        /// <summary>
        /// Adiciona uma observação do personal a um exercício.
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
                    retorno.Mensagem = "Exercício não encontrado para adicionar observação do personal.";
                    retorno.Resultado = false;
                }
                else
                {
                    retorno.Mensagem = "Observação do personal adicionada com sucesso.";
                    retorno.Resultado = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao adicionar observação do personal: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }

        /// <summary>
        /// Exclui um exercício pelo Id.
        /// </summary>
        public RetornoAcaoDto ExcluirExercicio(string idExercicio)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();

            try
            {
                var result = _database.DeleteOne(e => e.Id == idExercicio);

                if (result.DeletedCount == 0)
                {
                    retorno.Mensagem = "Exercício não encontrado para exclusão.";
                    retorno.Resultado = false;
                }
                else
                {
                    retorno.Mensagem = "Exercício excluído com sucesso.";
                    retorno.Resultado = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.Mensagem = $"Erro ao excluir exercício: {ex.Message}";
                retorno.Resultado = false;
                return retorno;
            }
        }
    }
}
