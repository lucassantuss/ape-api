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

            // Converte UTC para Brasília (UTC-3)
            var tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            var dataBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            entidade.Nome = dto.Nome;
            entidade.DataExecucao = dataBrasilia.ToString("dd/MM/yyyy - HH:mm:ss");
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
