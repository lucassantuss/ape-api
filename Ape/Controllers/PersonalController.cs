using Ape.Bll;
using Ape.Entity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly PersonalBll _personalBll;
        private readonly IMongoCollection<Aluno> _alunosCollection;

        public PersonalController(PersonalBll personalBll, IMongoCollection<Aluno> alunosCollection)
        {
            _personalBll = personalBll;
            _alunosCollection = alunosCollection;
        }

        // Criar personal
        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] PersonalDto dto)
        {
            var resultado = _personalBll.CriarPersonal(dto);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Redefinir Senha personal
        [HttpPut("RedefinirSenha")]
        public IActionResult RedefinirSenha([FromQuery] string usuario, [FromQuery] string novaSenha)
        {
            var resultado = _personalBll.RedefinirSenha(usuario, novaSenha);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Pesquisar personal por usuário
        [HttpGet("PesquisarPorUsuario/{usuario}")]
        public IActionResult PesquisarPorUsuario(string usuario)
        {
            var personal = _personalBll.PesquisarPersonalPorUsuario(usuario);
            return personal == null ? NotFound(new { mensagem = "Personal não encontrado" }) : Ok(personal);
        }

        // Pesquisar personal por id
        [HttpGet("PesquisarPorId/{id}")]
        public IActionResult PesquisarPorId(string id)
        {
            var personal = _personalBll.PesquisarPersonalPorId(id);
            return personal == null ? NotFound(new { mensagem = "Personal não encontrado" }) : Ok(personal);
        }

        // Pesquisar personal por Aluno
        [HttpGet("PesquisarPorAluno/{idAluno}")]
        public IActionResult PesquisarPorAluno(string idAluno)
        {
            var personal = _personalBll.PesquisarPersonalPorAluno(idAluno, _alunosCollection);
            return personal == null ? NotFound(new { mensagem = "Personal não encontrado para este aluno" }) : Ok(personal);
        }

        // Listar alunos por personal
        [HttpGet("ListarAlunos/{idPersonal}")]
        public IActionResult ListarAlunos(string idPersonal)
        {
            var alunos = _personalBll.PesquisarAlunosDoPersonal(idPersonal, _alunosCollection);
            return Ok(alunos);
        }

        // Alterar personal
        [HttpPut("Alterar")]
        public IActionResult Alterar([FromBody] PersonalDto dto)
        {
            var resultado = _personalBll.AlterarPersonal(dto);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Excluir personal
        [HttpDelete("Excluir/{id}")]
        public IActionResult Excluir(string id)
        {
            var resultado = _personalBll.ExcluirPersonal(id);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }
    }
}