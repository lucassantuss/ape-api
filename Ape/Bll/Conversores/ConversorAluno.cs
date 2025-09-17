using Ape.Dtos.Aluno;
using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorAluno
    {
        public Aluno ConverterAlunoDto(AlunoDto dto)
        {
            Aluno entidade = new Aluno();

            // Converte UTC para Brasília (UTC-3)
            var tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            var dataBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            entidade.Usuario = dto.Usuario;
            entidade.Nome = dto.Nome;
            entidade.Email = dto.Email;
            entidade.CPF = dto.CPF;
            entidade.Senha = dto.Senha;
            entidade.IdPersonal = dto.IdPersonal.ToString();

            entidade.AceitePersonal = false;
            entidade.DataAceitePersonal = "";

            entidade.AceiteTermoLGPD = dto.AceiteTermos;
            entidade.DataAceiteTermoLGPD = dataBrasilia;
            
            return entidade;
        }
    }
}