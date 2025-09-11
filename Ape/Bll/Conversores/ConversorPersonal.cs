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
            entidade.NumeroCREF = dto.NumeroCREF;
            entidade.CategoriaCREF = dto.CategoriaCREF;
            entidade.SiglaCREF = dto.SiglaCREF;

            entidade.AceiteTermoLGPD = true;
            entidade.DataAceiteTermoLGPD = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");

            return entidade;
        }
    }
}