using Microsoft.AspNetCore.Mvc;
using Ape.Dtos;
using Ape.Entity;
using Ape.Bll;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        #region Variáveis e Construtor
        private readonly ILogger<AlunoController> logger;
        private readonly AlunoBll alunoBll;

        // Construtor da controller que injeta as dependências de configuração e contexto do banco
        public AlunoController(ILogger<AlunoController> _logger, AlunoBll _alunoBll)
        {
            logger = _logger;
            alunoBll = _alunoBll;
        }

        #endregion

        #region Login - Métodos

        [HttpGet("PesquisarAluno")]
        public ActionResult<List<Aluno>> PesquisarAluno(AlunoDto alunoDto)
        {
            List<Aluno> aluno = new List<Aluno>();

            if (alunoDto == null)
                aluno = alunoBll.PesquisarAluno(alunoDto);

            return aluno;
        }

        [HttpPost("CriarAluno")]
        public RetornoAcaoDto CriarAluno([FromBody] AlunoDto alunoDto)
        {
            RetornoAcaoDto retorno = new RetornoAcaoDto();
            if (alunoDto != null)
            {
                retorno = alunoBll.CriarAluno(alunoDto);
            }
            else
            {
                retorno.Mensagem = "Falha ao criar o usuário";
                retorno.Sucesso = false;
            }
            return retorno;
        }

        #endregion
    }
}