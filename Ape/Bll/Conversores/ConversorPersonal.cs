using Ape.Entity;

namespace Ape.Bll.Conversores
{
    public class ConversorPersonal
    {
        public Personal ConverterPersonalDto(PersonalDto dto)
        {
            Personal entidade = new Personal();

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
            entidade.DataAceiteTermoLGPD = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss");

            return entidade;
        }
    }
}