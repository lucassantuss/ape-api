using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ape.Dtos;
using Ape.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ape.Bll;

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
        /// Realiza o login de um usuário (Aluno ou Personal).
        /// </summary>
        /// <param name="login">Objeto contendo usuário, senha e tipo de usuário ("aluno" ou "personal").</param>
        /// <returns>
        /// Retorna um token JWT para alunos ou um redirecionamento para o dashboard no caso de personal trainers.
        /// </returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Usuário ou senha inválidos.</response>
        [HttpPost("Entrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Entrar([FromBody] LoginDto login)
        {
            if (login.TipoUsuario == "aluno")
            {
                var aluno = _alunoBll.PesquisarAlunoLogin(login.Usuario, login.Senha);

                if (aluno != null)
                {
                    // Cria um manipulador de token JWT
                    var tokenHandler = new JwtSecurityTokenHandler();
                    // Lê a chave secreta para o JWT das configurações da aplicação
                    var jwtSecretKey = _configuration["Jwt_SecretKey"];
                    var key = Encoding.ASCII.GetBytes(jwtSecretKey);

                    // Configura os dados do token (claims, expiração e credenciais)
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.NameIdentifier, aluno.Id), // ID do usuário wsfsd
                                new Claim(ClaimTypes.Name, aluno.Usuario) // Login do usuário
                        }),
                        Expires = DateTime.UtcNow.AddHours(2), // Define a expiração do token para 2 horas
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Configura a assinatura com a chave secreta
                    };

                    // Gera o token JWT
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token); // Converte o token para string

                    // Retorna o Id do Usuário e o token JWT gerado como resposta
                    return Ok(new { idUser = aluno.Id, token = tokenString });
                }
                else
                {
                    return Unauthorized(new { message = "Usuário ou senha inválidos." });
                }
            }
            else if (login.TipoUsuario == "personal")
            {
                // TODO: fazer lógica parecida para personal
                return Unauthorized(new { message = "Login de personal ainda não implementado." });
            }

            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }

        #endregion
    }
}