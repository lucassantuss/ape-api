using Ape.Dtos.Aluno;
using Ape.Dtos.Exercicio;
using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorExercicio
    {
        public Exercicio ConverterExercicioDto(ExercicioDto dto)
        {
            Exercicio entidade = new Exercicio();

            entidade.Nome = dto.Nome;
            entidade.DataExecucao = DateTime.UtcNow;
            entidade.QuantidadeRepeticoes = dto.QuantidadeRepeticoes;
            entidade.PorcentagemAcertos = dto.PorcentagemAcertos;
            entidade.TempoExecutado = dto.TempoExecutado;
            entidade.ObservacoesAluno = dto.ObservacoesAluno;
            entidade.ObservacoesPersonal = dto.ObservacoesPersonal;
            entidade.IdAluno = dto.IdAluno;

            return entidade;
        }
    }
}
