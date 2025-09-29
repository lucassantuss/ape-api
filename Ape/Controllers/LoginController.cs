using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ape.Bll;
using Ape.Dtos.Login;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Variáveis e Construtor

        // Campos privados que armazenam a instância do banco de dados e as configurações
        private readonly ILogger<LoginController> _logger;
        private readonly PersonalBll _personalBll;
        private readonly AlunoBll _alunoBll;
        private readonly IConfiguration _configuration;

        // Construtor da controller que injeta as dependências de configuração e contexto do banco
        public LoginController(ILogger<LoginController> logger, PersonalBll personalBll, AlunoBll alunoBll, IConfiguration configuration)
        {
            _logger = logger;
            _personalBll = personalBll;
            _alunoBll = alunoBll;
            _configuration = configuration;
        }

        #endregion

        #region Login

        /// <summary>
        /// Realiza o login do aluno
        /// </summary>
        /// <param name="login">Objeto contendo usuário e senha</param>
        /// <returns>
        /// Retorna o id do usuário e o token JWT.
        /// </returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Usuário ou senha inválidos.</response>
        [HttpPost("aluno")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult LoginAluno([FromBody] LoginDto login)
        {
            var aluno = _alunoBll.PesquisarAlunoLogin(login.Usuario, login.Senha);

            if (aluno == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            var token = GerarToken(aluno.Id, aluno.Usuario);
            return Ok(new { idUser = aluno.Id, token });
        }

        /// <summary>
        /// Realiza o login do personal trainer
        /// </summary>
        /// <param name="login">Objeto contendo usuário e senha</param>
        /// <returns>
        /// Retorna o id do usuário e o token JWT.
        /// </returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Usuário ou senha inválidos.</response>
        [HttpPost("personal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult LoginPersonal([FromBody] LoginDto login)
        {
            var personal = _personalBll.PesquisarPersonalLogin(login.Usuario, login.Senha);

            if (personal == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            var token = GerarToken(personal.Id, personal.Usuario);
            return Ok(new { idUser = personal.Id, token });
        }

        /// <summary>
        /// Método utilitário para gerar JWT.
        /// </summary>
        private string GerarToken(string id, string usuario)
        {
            // Cria um manipulador de token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            // Lê a chave secreta para o JWT das configurações da aplicação
            var jwtSecretKey = _configuration["Jwt_SecretKey"];
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            // Configura os dados do token (claims, expiração e credenciais)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id), // ID do usuário
                    new Claim(ClaimTypes.Name, usuario) // Login do usuário
                }),
                Expires = DateTime.Now.AddHours(6), // Define a expiração do token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature) // Configura a assinatura com a chave secreta
            };

            // Gera o token JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}