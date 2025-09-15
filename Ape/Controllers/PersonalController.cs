using Ape.Bll;
using Ape.Dtos.Personal;
using Ape.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]    
    public class PersonalController : ControllerBase
    {
        private readonly PersonalBll _personalBll;
        private readonly AlunoBll _alunoBll;
        private readonly IMongoCollection<Aluno> _alunosCollection;

        public PersonalController(AlunoBll alunoBll, PersonalBll personalBll, IMongoCollection<Aluno> alunosCollection)
        {
            _alunoBll = alunoBll;
            _personalBll = personalBll;
            _alunosCollection = alunosCollection;
        }

        /// <summary>
        /// Cria um novo personal trainer.
        /// </summary>
        /// <param name="dto">Dados do personal a ser criado.</param>
        /// <returns>Resultado da operação (sucesso ou erro).</returns>
        /// <response code="200">Personal criado com sucesso.</response>
        /// <response code="400">Erro ao criar personal.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Criar([FromBody] PersonalDto dto)
        {
            RetornoAcaoDto resultado = new RetornoAcaoDto();

            if (dto != null)
                resultado = _personalBll.CriarPersonal(dto);

            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Lista todos os personais cadastrados.
        /// </summary>
        /// <returns>Lista de personais.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Listar()
        {
            var lista = _personalBll.ListarPersonais();
            return Ok(lista);
        }

        /// <summary>
        /// Pesquisa um personal pelo seu ID.
        /// </summary>
        /// <param name="id">ID do personal.</param>
        /// <returns>Dados do personal encontrado.</returns>
        /// <response code="200">Personal encontrado.</response>
        /// <response code="404">Personal não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarPorId(string id)
        {
            var personal = _personalBll.PesquisarPersonalPorId(id);
            return personal == null
                ? NotFound(new { mensagem = "Personal não encontrado" })
                : Ok(personal);
        }

        /// <summary>
        /// Pesquisa um personal pelo nome de usuário.
        /// </summary>
        /// <param name="usuario">Usuário do personal.</param>
        /// <returns>Dados do personal encontrado.</returns>
        /// <response code="200">Personal encontrado.</response>
        /// <response code="404">Personal não encontrado.</response>
        [HttpGet("usuario/{usuario}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarPorUsuario(string usuario)
        {
            var personal = _personalBll.PesquisarPersonalPorUsuario(usuario);
            return personal == null
                ? NotFound(new { mensagem = "Personal não encontrado" })
                : Ok(personal);
        }

        /// <summary>
        /// Altera os dados de um personal existente.
        /// </summary>
        /// <param name="id">ID do personal.</param>
        /// <param name="dto">Dados atualizados do personal.</param>
        /// <returns>Resultado da operação (sucesso ou erro).</returns>
        /// <response code="200">Personal alterado com sucesso.</response>
        /// <response code="400">Erro ao alterar personal.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Alterar(string id, [FromBody] AlterarPersonalDto dto)
        {
            var resultado = _personalBll.AlterarPersonal(id, dto);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Exclui um personal pelo ID.
        /// </summary>
        /// <param name="id">ID do personal.</param>
        /// <returns>Resultado da operação.</returns>
        /// <response code="200">Personal excluído com sucesso.</response>
        /// <response code="400">Erro ao excluir personal.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Excluir(string id)
        {
            var resultado = _personalBll.ExcluirPersonal(id);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Redefine a senha de um personal.
        /// </summary>
        /// <param name="usuario">Usuário do personal.</param>
        /// <param name="novaSenha">Nova senha do personal (via query string).</param>
        /// <returns>Resultado da operação.</returns>
        /// <response code="200">Senha redefinida com sucesso.</response>
        /// <response code="400">Erro ao redefinir senha.</response>
        [HttpPut("{usuario}/senha")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RedefinirSenha(string usuario, [FromQuery] string novaSenha)
        {
            var resultado = _personalBll.RedefinirSenha(usuario, novaSenha);
            return resultado.Resultado ? Ok(resultado) : BadRequest(resultado);
        }

        /// <summary>
        /// Lista todos os alunos vinculados a um personal.
        /// </summary>
        /// <param name="idPersonal">ID do personal.</param>
        /// <returns>Lista de alunos do personal.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet("{idPersonal}/alunos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ListarAlunos(string idPersonal)
        {
            var alunos = _personalBll.PesquisarAlunosDoPersonal(idPersonal, _alunosCollection);
            return Ok(alunos);
        }

        /// <summary>
        /// Pesquisa o personal associado a um aluno.
        /// </summary>
        /// <param name="idAluno">ID do aluno.</param>
        /// <returns>Dados do personal vinculado.</returns>
        /// <response code="200">Personal encontrado.</response>
        /// <response code="404">Nenhum personal vinculado a este aluno.</response>
        [HttpGet("aluno/{idAluno}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarPorAluno(string idAluno)
        {
            var personal = _personalBll.PesquisarPersonalPorAluno(idAluno, _alunosCollection);
            return personal == null
                ? NotFound(new { mensagem = "Personal não encontrado para este aluno" })
                : Ok(personal);
        }

        /// <summary>
        /// Aceita a solicitação de vínculo do aluno pelo personal.
        /// </summary>
        [HttpPost("aceitar/{idAluno}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AceitarAluno(string idAluno)
        {
            var resultado = _alunoBll.AceitarAluno(idAluno);
            if (resultado.Resultado)
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Recusa a solicitação de vínculo do aluno pelo personal.
        /// </summary>
        [HttpPost("recusar/{idAluno}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RecusarAluno(string idAluno)
        {
            var resultado = _alunoBll.RecusarAluno(idAluno);
            if (resultado.Resultado == false)
                return Ok(resultado);

            return NotFound(resultado);
        }
    }
}
