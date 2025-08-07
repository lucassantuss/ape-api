using Microsoft.AspNetCore.Mvc;
using Ape.Bll;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        #region Variáveis e Construtor
        private readonly ILogger<PersonalController> logger;
        private readonly PersonalBll personalBll;

        // Construtor da controller que injeta as dependências de configuração e contexto do banco
        public PersonalController(ILogger<PersonalController> _logger, PersonalBll _personalBll)
        {
            logger = _logger;
            personalBll = _personalBll;
        }

        #endregion

        #region Login - Métodos

        #endregion
    }
}