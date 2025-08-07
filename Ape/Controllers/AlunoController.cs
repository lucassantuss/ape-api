using Ape.Bll;
using Ape.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly AlunoBll _alunoBll;

        public AlunoController(AlunoBll alunoBll)
        {
            _alunoBll = alunoBll;
        }

        // Criar um novo aluno
        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] AlunoDto dto)
        {
            var resultado = _alunoBll.CriarAluno(dto);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        // Redefinir senha de um aluno
        [HttpPut("RedefinirSenha")]
        public IActionResult RedefinirSenha([FromQuery] string usuario, [FromQuery] string novaSenha)
        {
            var resultado = _alunoBll.RedefinirSenha(usuario, novaSenha);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        // Pesquisar aluno por usuário
        [HttpGet("PesquisarPorUsuario/{usuario}")]
        public IActionResult PesquisarPorUsuario(string usuario)
        {
            var aluno = _alunoBll.PesquisarAlunoPorUsuario(usuario);
            if (aluno == null)
                return NotFound(new { mensagem = "Aluno não encontrado." });

            return Ok(aluno);
        }

        // Pesquisar aluno por ID
        [HttpGet("PesquisarPorId/{id}")]
        public IActionResult PesquisarPorId(string id)
        {
            var aluno = _alunoBll.PesquisarAlunoPorId(id);
            if (aluno == null)
                return NotFound(new { mensagem = "Aluno não encontrado." });

            return Ok(aluno);
        }

        // Alterar aluno
        [HttpPut("Alterar")]
        public IActionResult Alterar([FromBody] AlunoDto dto)
        {
            var resultado = _alunoBll.AlterarAluno(dto);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        // Excluir aluno por ID
        [HttpDelete("Excluir/{id}")]
        public IActionResult Excluir(string id)
        {
            var resultado = _alunoBll.ExcluirAluno(id);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }
    }
}