using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ape.Database;
using Ape.Dtos;
using Ape.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Ape.Bll;

namespace Ape.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Variáveis e Construtor

        // Campos privados que armazenam a instância do banco de dados e as configurações
        private readonly ILogger<LoginController> logger;
        private readonly PersonalBll personalBll;

        // Construtor da controller que injeta as dependências de configuração e contexto do banco
        public LoginController(ILogger<LoginController> _logger, PersonalBll _personalBll)
        {
            logger = _logger;
            personalBll = _personalBll;
        }

        #endregion

        #region Login

        [HttpPost("Entrar")]
        public IActionResult Entrar([FromBody] LoginDto login)
        {
            if (login.Usuario == "cr7")
                return Ok();
            else
                return NotFound();

            if(login.TipoUsuario == "aluno")
            {

            }
            else if(login.TipoUsuario == "personal")
            {

            }
        }

        #endregion

        //#region Login - Métodos

        //#region Perfil

        //#region Listar Perfis
        //// Endpoint para listar todos os perfis armazenados no banco de dados
        //[HttpGet("ListarPerfis")]
        //public ActionResult<IEnumerable<Perfil>> ListarPerfis()
        //{
        //    return _dbApe.Perfil.ToList();
        //}
        //#endregion

        //#region Pesquisar Perfil por Id
        //// Endpoint para pesquisar um perfil específico pelo ID
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpGet("PesquisarPerfil/{id}")]
        //public ActionResult<Perfil> PesquisarPerfil(int id)
        //{
        //    var perfil = _dbApe.Perfil.Find(id);

        //    if (perfil == null)
        //    {
        //        return NotFound(); // Retorna 404 se o perfil não for encontrado
        //    }

        //    return perfil;
        //}
        //#endregion

        //#region Criar Perfil
        //// Endpoint para criar um novo perfil com base nos dados enviados
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpPost("CriarPerfil")]
        //public ActionResult<Perfil> CriarPerfil(PerfilDto perfilDto)
        //{
        //    // Verifica se o nome e descrição do perfil são válidos
        //    if (string.IsNullOrWhiteSpace(perfilDto.NomePerfil) || string.IsNullOrWhiteSpace(perfilDto.DescricaoPerfil))
        //        return BadRequest("O Nome ou Descrição do Perfil não pode ser vazio."); // Retorna erro 400 se inválido

        //    // Cria um novo objeto de perfil com os dados fornecidos
        //    Perfil perfil = new Perfil
        //    {
        //        NomePerfil = perfilDto.NomePerfil,
        //        DescricaoPerfil = perfilDto.DescricaoPerfil
        //    };

        //    // Adiciona o novo perfil ao banco de dados e salva as alterações
        //    _dbApe.Perfil.Add(perfil);
        //    _dbApe.SaveChanges();

        //    // Retorna o perfil criado com a localização (URL) do recurso criado
        //    return CreatedAtAction(nameof(PesquisarPerfil), new { id = perfil.IdPerfil }, perfil);
        //}
        //#endregion

        //#region Alterar Perfil
        //// Endpoint para alterar os dados de um perfil existente pelo ID
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpPut("AlterarPerfil/{id}")]
        //public IActionResult AlterarPerfil(int id, PerfilDto perfilDto)
        //{
        //    var perfil = _dbApe.Perfil.Find(id);

        //    if (perfil == null)
        //    {
        //        return NotFound(); // Retorna 404 se o perfil não for encontrado
        //    }

        //    // Atualiza os dados do perfil com os valores fornecidos
        //    perfil.NomePerfil = perfilDto.NomePerfil;
        //    perfil.DescricaoPerfil = perfilDto.DescricaoPerfil;

        //    // Salva as alterações no banco de dados
        //    _dbApe.SaveChanges();

        //    // Retorna o perfil atualizado como resposta
        //    return Ok(perfil);
        //}
        //#endregion

        //#region Excluir Perfil
        //// Endpoint para excluir um perfil específico pelo ID
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpDelete("ExcluirPerfil/{id}")]
        //public IActionResult ExcluirPerfil(int id)
        //{
        //    // Busca o perfil pelo ID no banco de dados
        //    var perfil = _dbApe.Perfil.Find(id);

        //    if (perfil == null)
        //    {
        //        return NotFound(); // Retorna 404 se o perfil não for encontrado
        //    }

        //    _dbApe.Perfil.Remove(perfil);
        //    _dbApe.SaveChanges();

        //    return Ok("Perfil excluído com sucesso!");
        //}
        //#endregion

        //#endregion

        //#region Usuário

        //#region Listar Usuários
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpGet("ListarUsuarios")]
        //public ActionResult<IEnumerable<UsuarioSimplesDto>> ListarUsuarios()
        //{
        //    var usuarios = _dbApe.Usuario
        //                         .Select(xs => new UsuarioSimplesDto
        //                         {
        //                             IdUsuario = xs.IdUsuario,
        //                             Login = xs.Login,
        //                         })
        //                         .ToList();

        //    return usuarios;
        //}
        //#endregion

        //#region Pesquisar Usuário por Id
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpGet("PesquisarUsuario/{id}")]
        //public ActionResult<UsuarioSimplesDto> PesquisarUsuario(int id)
        //{
        //    var usuario = _dbApe.Usuario.Find(id);

        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    UsuarioSimplesDto user = new UsuarioSimplesDto
        //    {
        //        IdUsuario = usuario.IdUsuario,
        //        Login = usuario.Login,
        //    };

        //    return user;
        //}
        //#endregion

        //#region Criar Usuário
        //[HttpPost("CriarUsuario")]
        //public ActionResult<Usuario> CriarUsuario(UsuarioDto novoUsuarioDto)
        //{
        //    // Validação dos dados de entrada
        //    if (novoUsuarioDto == null)
        //        return BadRequest("Os dados do usuário não podem ser nulos.");

        //    // Verifica se já existe um usuário com o mesmo Login
        //    if (_dbApe.Usuario.Any(u => u.Login.ToUpper() == novoUsuarioDto.Login.ToUpper()))
        //        return BadRequest("Já existe um usuário com esse login.");

        //    // Criação do usuário
        //    if (string.IsNullOrEmpty(novoUsuarioDto.Login))
        //        return BadRequest("O login do usuário é obrigatório.");

        //    if (string.IsNullOrEmpty(novoUsuarioDto.Senha))
        //        return BadRequest("A senha do usuário é obrigatória.");

        //    Usuario usuario = new Usuario
        //    {
        //        Login = novoUsuarioDto.Login,
        //        Senha = BCrypt.Net.BCrypt.HashPassword(novoUsuarioDto.Senha),
        //    };

        //    _dbApe.Usuario.Add(usuario);
        //    _dbApe.SaveChanges();

        //    // Adiciona perfil padrão para o usuário
        //    var usuarioPerfil = new UsuarioPerfil()
        //    {
        //        IdUsuario = usuario.IdUsuario,
        //        IdPerfil = 2 // Perfil Cliente
        //    };
        //    _dbApe.UsuarioPerfil.Add(usuarioPerfil);
        //    _dbApe.SaveChanges();

        //    return CreatedAtAction(nameof(PesquisarUsuario), new { id = usuario.IdUsuario }, usuario);
        //}
        //#endregion

        //#region Entrar
        //// Endpoint para login e geração de um token JWT para autenticação
        //[HttpPost("Entrar")]
        //public IActionResult Entrar(LoginDto loginDto)
        //{
        //    if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Login) ||
        //        string.IsNullOrWhiteSpace(loginDto.Senha))
        //        return BadRequest("Digite o Login e senha corretamente!");

        //    // Verifica se o usuário existe
        //    var usuario = _dbApe.Usuario
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
        //#endregion

        //#region Redefinir Senha
        //[HttpPut("RedefinirSenha/{id}")]
        //public IActionResult RedefinirSenha(int id, NovaSenhaDto novaSenhaDto)
        //{
        //    // Busca o usuário pelo ID fornecido
        //    var usuario = _dbApe.Usuario.Find(id);

        //    // Se o usuário não for encontrado, retorna um status 404 NotFound
        //    if (usuario == null)
        //    {
        //        return NotFound("Usuário não encontrado.");
        //    }

        //    // Atualiza a senha do usuário com a nova senha fornecida
        //    usuario.Senha = novaSenhaDto.NovaSenha; // TODO Criptografar a senha

        //    // Salva as alterações no banco de dados
        //    _dbApe.SaveChanges();

        //    // Retorna um status 200 OK com uma mensagem de sucesso
        //    return Ok("Senha alterada com sucesso.");
        //}
        //#endregion

        //#region Excluir Usuário
        //[Authorize] // Requer autorização para acessar este endpoint
        //[HttpDelete("ExcluirUsuario/{id}")]
        //public IActionResult ExcluirUsuario(int id)
        //{
        //    // Busca o usuário pelo ID fornecido
        //    var usuario = _dbApe.Usuario.Find(id);

        //    // Se o usuário não for encontrado, retorna um status 404 NotFound
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    // Busca o perfil de usuário associado
        //    var usuarioPerfil = _dbApe.UsuarioPerfil.Find(usuario.IdUsuario);

        //    // Se o perfil do usuário não for encontrado, retorna NotFound
        //    if (usuarioPerfil == null)
        //    {
        //        return NotFound();
        //    }

        //    // Remove o perfil do usuário e o usuário do banco de dados
        //    _dbApe.UsuarioPerfil.Remove(usuarioPerfil);
        //    _dbApe.Usuario.Remove(usuario);

        //    // Salva as alterações no banco de dados
        //    _dbApe.SaveChanges();

        //    // Retorna um status 200 OK com uma mensagem de sucesso
        //    return Ok("Usuário excluído com sucesso!");
        //}
        //#endregion

        //#endregion

        //#endregion

        //#region Utils
        //// Funções auxiliares para validações de CNPJ e Email
        //private bool IsValidCNPJ(string cnpj)
        //{
        //    return Regex.IsMatch(cnpj, @"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$");
        //}

        //private bool IsValidEmail(string email)
        //{
        //    return Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
        //}
        //#endregion
    }
}