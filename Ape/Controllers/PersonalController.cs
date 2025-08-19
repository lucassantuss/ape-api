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
        [HttpPost]
        public IActionResult Criar([FromBody] PersonalDto dto)
        {
            var resultado = _personalBll.CriarPersonal(dto);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Pesquisar todos
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _personalBll.ListarPersonais();
            return Ok(lista);
        }

        // Pesquisar por ID
        [HttpGet("{id}")]
        public IActionResult PesquisarPorId(string id)
        {
            var personal = _personalBll.PesquisarPersonalPorId(id);
            return personal == null
                ? NotFound(new { mensagem = "Personal não encontrado" })
                : Ok(personal);
        }

        // Pesquisar por usuário
        [HttpGet("usuario/{usuario}")]
        public IActionResult PesquisarPorUsuario(string usuario)
        {
            var personal = _personalBll.PesquisarPersonalPorUsuario(usuario);
            return personal == null
                ? NotFound(new { mensagem = "Personal não encontrado" })
                : Ok(personal);
        }

        // Alterar personal
        [HttpPut("{id}")]
        public IActionResult Alterar(string id, [FromBody] PersonalDto dto)
        {
            var resultado = _personalBll.AlterarPersonal(id, dto);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Excluir personal
        [HttpDelete("{id}")]
        public IActionResult Excluir(string id)
        {
            var resultado = _personalBll.ExcluirPersonal(id);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Redefinir senha (ação específica → mantenho separado)
        [HttpPut("{usuario}/senha")]
        public IActionResult RedefinirSenha(string usuario, [FromQuery] string novaSenha)
        {
            var resultado = _personalBll.RedefinirSenha(usuario, novaSenha);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        // Listar alunos de um personal
        [HttpGet("{idPersonal}/alunos")]
        public IActionResult ListarAlunos(string idPersonal)
        {
            var alunos = _personalBll.PesquisarAlunosDoPersonal(idPersonal, _alunosCollection);
            return Ok(alunos);
        }

        // Pesquisar personal de um aluno
        [HttpGet("aluno/{idAluno}")]
        public IActionResult PesquisarPorAluno(string idAluno)
        {
            var personal = _personalBll.PesquisarPersonalPorAluno(idAluno, _alunosCollection);
            return personal == null
                ? NotFound(new { mensagem = "Personal não encontrado para este aluno" })
                : Ok(personal);
        }
    }
}
