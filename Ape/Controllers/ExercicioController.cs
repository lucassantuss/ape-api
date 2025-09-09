using Ape.Bll;
using Ape.Dtos.Exercicio;
using Ape.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ape.Controllers
{
    /// <summary>
    /// Controller responsável pelos exercícios
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ExercicioController : ControllerBase
    {
        private readonly ExercicioBll _exercicioBll;

        public ExercicioController(ExercicioBll exercicioBll)
        {
            _exercicioBll = exercicioBll;
        }

        /// <summary>
        /// Salva um novo exercício realizado no sistema.
        /// </summary>
        [HttpPost("salvarResultados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SalvarResultados([FromBody] ExercicioDto exercicioDto)
        {
            var resultado = _exercicioBll.SalvarResultados(exercicioDto);

            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Pesquisa um exercício pelo ID.
        /// </summary>
        [HttpGet("pesquisarPorId/{idExercicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PesquisarPorId(string idExercicio)
        {
            var exercicio = _exercicioBll.PesquisarPorId(idExercicio);
            if (exercicio == null)
                return NotFound(new { mensagem = "Exercício não encontrado." });

            return Ok(exercicio);
        }

        /// <summary>
        /// Lista todos os exercícios realizados por um aluno.
        /// </summary>
        [HttpGet("listByIdUser/{idUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ListarPorIdUser(string idUser)
        {
            var lista = _exercicioBll.ListarPorIdUser(idUser);
            return Ok(lista);
        }

        /// <summary>
        /// Adiciona observação do aluno em um exercício.
        /// </summary>
        [HttpPut("adicionarObservacaoAluno/{idExercicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AdicionarObservacaoAluno(string idExercicio, [FromBody] string observacao)
        {
            var resultado = _exercicioBll.AdicionarObservacaoAluno(idExercicio, observacao);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Adiciona observação do personal em um exercício.
        /// </summary>
        [HttpPut("adicionarObservacaoPersonal/{idExercicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AdicionarObservacaoPersonal(string idExercicio, [FromBody] string observacao)
        {
            var resultado = _exercicioBll.AdicionarObservacaoPersonal(idExercicio, observacao);
            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Exclui um exercício do histórico.
        /// </summary>
        [HttpDelete("{idExercicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ExcluirExercicio(string idExercicio)
        {
            var resultado = _exercicioBll.ExcluirExercicio(idExercicio);

            if (resultado.Resultado)
                return Ok(resultado);

            return BadRequest(resultado);
        }
    }
}