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

        [HttpPost("Entrar")]
        public IActionResult Entrar([FromBody] LoginDto login)
        {
            if (login.TipoUsuario == "aluno")
            {
                AlunoDto dto = new AlunoDto();
                dto.Usuario = login.Usuario;
                List<Aluno> aluno = _alunoBll.PesquisarAluno(dto);
                if (aluno != null && 
                   (aluno[0].Usuario == login.Usuario && 
                    aluno[0].Senha == login.Senha))
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
                                new Claim(ClaimTypes.NameIdentifier, aluno[0].Id), // ID do usuário wsfsd
                                new Claim(ClaimTypes.Name, aluno[0].Usuario) // Login do usuário
                        }),
                        Expires = DateTime.UtcNow.AddHours(2), // Define a expiração do token para 2 horas
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Configura a assinatura com a chave secreta
                    };

                    // Gera o token JWT
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token); // Converte o token para string

                    // Retorna o Id do Usuário e o token JWT gerado como resposta
                    return Ok(new { redirectTo = "minhaconta/index", idUser = aluno[0].Id, token = tokenString });
                }
                else
                {
                    return Unauthorized(new { message = "Usuário ou senha inválidos." });
                }
            }
            else if (login.TipoUsuario == "personal")
            {
                AlunoDto dto = new AlunoDto();
                dto.Usuario = login.Usuario;
                List<Aluno> aluno = _alunoBll.PesquisarAluno(dto);
                if (aluno != null)
                    return Ok(new { redirectTo = "/dashboard" }); // Caminho do frontend
                else
                    return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }
            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }

        #endregion
    }
}