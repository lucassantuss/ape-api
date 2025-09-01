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
                throw new Exception($"Erro ao pesquisar exercício: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os exercícios de um aluno específico.
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
    }
}
