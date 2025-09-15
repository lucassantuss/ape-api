using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorPersonal
    {
        public Personal ConverterPersonalDto(PersonalDto dto)
        {
            Personal entidade = new Personal();

            // Converte UTC para Brasília (UTC-3)
            var tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            var dataBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            entidade.Nome = dto.Nome;
            entidade.Usuario = dto.Usuario;
            entidade.Email = dto.Email;
            entidade.Senha = dto.Senha;
            entidade.CPF = dto.CPF;
            entidade.Estado = dto.Estado;
            entidade.Cidade = dto.Cidade;
            entidade.NumeroCref = dto.NumeroCref;
            entidade.CategoriaCref = dto.CategoriaCref;
            entidade.SiglaCref = dto.SiglaCref;

            entidade.AceiteTermoLGPD = true;
            entidade.DataAceiteTermoLGPD = dataBrasilia.ToString("dd/MM/yyyy - HH:mm:ss");

            return entidade;
        }
    }
}