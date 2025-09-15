using Ape.Bll;
using Ape.Dtos.Aluno;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ape.Controllers
{
    /// <summary>
    /// Controller de Alunos
    /// </summary>
    [Route("[controller]")]
    [ApiController]    
    public class AlunoController : ControllerBase
    {
        private readonly AlunoBll _alunoBll;

        /// <summary>
        /// Construtor para instanciar a Bll
        /// </summary>
        /// <param name="alunoBll"></param>
        public AlunoController(AlunoBll alunoBll)
        {
            _alunoBll = alunoBll;
        }

        /// <summary>
        /// Cria um novo aluno no sistema.
        /// </summary>
        /// <param name="dto">Dados do aluno (usuário, senha, nome, e-mail, etc).</param>
        /// <returns>Retorna sucesso ou erro com mensagem.</returns>
        /// <response code="200">Aluno criado com sucesso.</response>
        /// <response code="400">Erro ao criar aluno (ex: usuário já existe).</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Criar([FromBody] AlunoDto dto)
        {
            RetornoAcaoDto resultado = new RetornoAcaoDto();

            if(dto != null)
                resultado = _alunoBll.CriarAluno(dto);

            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Redefine a senha de um aluno.
        /// </summary>
        /// <param name="usuario">Nome de usuário do aluno.</param>
        /// <param name="novaSenha">Nova senha.</param>
        /// <returns>Retorna sucesso ou erro.</returns>
        /// <response code="200">Senha redefinida com sucesso.</response>
        /// <response code="400">Erro ao redefinir senha.</response>
        [HttpPut("{usuario}/senha")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RedefinirSenha(string usuario, [FromQuery] string novaSenha)
        {
            var resultado = _alunoBll.RedefinirSenha(usuario, novaSenha);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Pesquisa um aluno pelo nome de usuário.
        /// </summary>
        /// <param name="usuario">Usuário do aluno.</param>
        /// <returns>Retorna os dados do aluno.</returns>
        /// <response code="200">Aluno encontrado.</response>
        /// <response code="404">Aluno não encontrado.</response>
        [HttpGet("usuario/{usuario}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarPorUsuario(string usuario)
        {
            var aluno = _alunoBll.PesquisarAlunoPorUsuario(usuario);
            if (aluno == null)
                return NotFound(new { mensagem = "Aluno não encontrado." });

            return Ok(aluno);
        }

        /// <summary>
        /// Pesquisa um aluno pelo ID.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <returns>Retorna os dados do aluno.</returns>
        /// <response code="200">Aluno encontrado.</response>
        /// <response code="404">Aluno não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarPorId(string id)
        {
            var aluno = _alunoBll.PesquisarAlunoPorId(id);
            if (aluno == null)
                return NotFound(new { mensagem = "Aluno não encontrado." });

            return Ok(aluno);
        }

        /// <summary>
        /// Pesquisar se o personal aceitou o aluno.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <returns>Retorna os dados do aluno.</returns>
        /// <response code="200">Aluno encontrado.</response>
        /// <response code="404">Aluno não encontrado.</response>
        [HttpGet("/Aluno/personal/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarAceitePersonalPorIdAluno(string id)
        {
            var retorno = _alunoBll.PesquisarAceitePersonalPorIdAluno(id);
            return Ok(retorno);
        }

        /// <summary>
        /// Altera os dados de um aluno.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <param name="dto">Novos dados do aluno.</param>
        /// <returns>Retorna sucesso ou erro.</returns>
        /// <response code="200">Aluno atualizado com sucesso.</response>
        /// <response code="400">Erro ao atualizar aluno.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Alterar(string id, [FromBody] AlterarAlunoDto dto)
        {
            var resultado = _alunoBll.AlterarAluno(id, dto);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Exclui um aluno pelo ID.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <returns>Retorna sucesso ou erro.</returns>
        /// <response code="200">Aluno excluído com sucesso.</response>
        /// <response code="400">Erro ao excluir aluno.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Excluir(string id)
        {
            var resultado = _alunoBll.ExcluirAluno(id);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Remove o vínculo do Personal de um aluno.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <returns>Retorna sucesso ou erro.</returns>
        /// <response code="200">Personal desvinculado com sucesso.</response>
        /// <response code="400">Erro ao remover vínculo.</response>
        [HttpPut("{id}/remover-personal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RemoverPersonal(string id)
        {
            var resultado = _alunoBll.RemoverPersonal(id);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }
    }
}