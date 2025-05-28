using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ape.Database;
using Ape.Dtos;
using Ape.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        #region Variáveis e Construtor

        // Campos privados que armazenam a instância do banco de dados
        private readonly MongoDbContext dbApe;

        // Construtor da controller que injeta as dependências de configuração e contexto do banco
        public AlunoController(MongoDbContext dbApe)
        {
            this.dbApe = dbApe;
        }

        #endregion

        #region Login - Métodos

        #region Aluno

        #region Pesquisa Aluno

        [HttpGet("PesquisarAluno")]
        public ActionResult<List<Aluno>> PesquisarAluno(Aluno Aluno)
        {
            try
            {
                if (Aluno.Nome != null)
                {
                    return dbApe.Aluno.Find(Aluno.Nome).ToList();
                }
                return dbApe.Aluno.Find(_ => true).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        [HttpGet("PesquisarAlunoPorId/{id}")]
        public ActionResult<Aluno> PesquisarAlunoPorId(string id)
        {
            try
            {
                Aluno Aluno = dbApe.Aluno.Find(p => p.Id == id).FirstOrDefault();
                if (Aluno == null)
                    return NotFound("Aluno não encontrado.");
                else
                    return Aluno;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost("CriarAluno")]
        public ActionResult<Aluno> CriarAluno(Aluno Aluno)
        {
            try
            {
                dbApe.Aluno.InsertOne(Aluno);
                return CreatedAtAction(nameof(PesquisarAlunoPorId), new { id = Aluno.Id }, Aluno);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut("AtualizarAluno")]
        public ActionResult AtualizarAluno(Aluno dtoAluno)
        {
            try
            {
                Aluno Aluno = dbApe.Aluno.Find(dtoAluno.Id).FirstOrDefault();
                if (Aluno == null)
                    return NotFound("Aluno não encontrado.");
                else
                {
                    dbApe.Aluno.ReplaceOne(p => p.Id == dtoAluno.Id, dtoAluno);
                    return NoContent();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("DeletarAluno")]
        public ActionResult DeletarAluno(Aluno dtoAluno)
        {
            try
            {
                Aluno Aluno = dbApe.Aluno.Find(dtoAluno.Id).FirstOrDefault();
                if (Aluno == null)
                    return NotFound("Aluno não encontrado.");
                else
                {
                    dbApe.Aluno.DeleteOne(p => p.Id == dtoAluno.Id);
                    return NoContent();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Entrar
        // Endpoint para login e geração de um token JWT para autenticação
        //[HttpPost("Entrar")]
        //public IActionResult Entrar(LoginDto loginDto)
        //{
        //    if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Login) ||
        //        string.IsNullOrWhiteSpace(loginDto.Senha))
        //        return BadRequest("Digite o Login e senha corretamente!");

        //    // Verifica se o usuário existe
        //    var usuario = dbApe.Usuario
        //                        .SingleOrDefault(u => u.Login == loginDto.Login);

        //    // Retorna erro 401 se o login ou senha estiver incorreto
        //    if (usuario == null)
        //        return Unauthorized("Login e/ou senha inválidos.");

        //    // Verifica se a senha digitada é igual à senha criptografada no banco
        //    bool senhaValidada = BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.Senha);

        //    // Retorna erro 401 se a senha for diferente da senha salva no banco
        //    if (!senhaValidada)
        //        return Unauthorized("Login e/ou senha inválidos.");

        //    // Cria um manipulador de token JWT
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    // Lê a chave secreta para o JWT das configurações da aplicação
        //    var jwtSecretKey = _configuration["Jwt_SecretKey"];
        //    var key = Encoding.ASCII.GetBytes(jwtSecretKey);

        //    // Configura os dados do token (claims, expiração e credenciais)
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()), // ID do usuário
        //            new Claim(ClaimTypes.Name, usuario.Login) // Login do usuário
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(2), // Define a expiração do token para 2 horas
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Configura a assinatura com a chave secreta
        //    };

        //    // Gera o token JWT
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token); // Converte o token para string

        //    // Retorna o Id do Usuário e o token JWT gerado como resposta
        //    return Ok(new { Token = tokenString, IdUser = usuario.IdUsuario });
        //}
        #endregion

        #region Redefinir Senha
        //[HttpPut("RedefinirSenha/{id}")]
        //public IActionResult RedefinirSenha(int id, NovaSenhaDto novaSenhaDto)
        //{
        //    // Busca o usuário pelo ID fornecido
        //    var usuario = dbApe.Usuario.Find(id);

        //    // Se o usuário não for encontrado, retorna um status 404 NotFound
        //    if (usuario == null)
        //    {
        //        return NotFound("Usuário não encontrado.");
        //    }

        //    // Atualiza a senha do usuário com a nova senha fornecida
        //    usuario.Senha = novaSenhaDto.NovaSenha; // TODO Criptografar a senha

        //    // Salva as alterações no banco de dados
        //    dbApe.SaveChanges();

        //    // Retorna um status 200 OK com uma mensagem de sucesso
        //    return Ok("Senha alterada com sucesso.");
        //}
        #endregion

        #endregion

        #region Utils
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
        }
        #endregion
    }
}
