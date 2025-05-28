using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ape.Database;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura��es do JWT - Define a chave secreta para o token JWT, que pode ser obtida das vari�veis de ambiente ou do arquivo de configura��o.
var jwtSecretKey = Environment.GetEnvironmentVariable("Jwt_SecretKey")
    ?? builder.Configuration["Jwt_SecretKey"];

// Verifica se a chave secreta do JWT est� definida; se n�o estiver, lan�a uma exce��o.
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("A chave secreta JWT n�o est� definida.");
}

// Converte a chave secreta para um array de bytes para ser utilizada na configura��o do token.
var key = Encoding.ASCII.GetBytes(jwtSecretKey);

// Configura o servi�o de autentica��o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema padr�o para autentica��o
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // Define o esquema padr�o para desafios de autentica��o
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Desativa o requisito de HTTPS para obter tokens (�til para desenvolvimento)
    options.SaveToken = true;             // Salva o token JWT no contexto da autentica��o
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,           // Desabilita a valida��o do emissor do token
        ValidateAudience = false,         // Desabilita a valida��o da audi�ncia do token
        ValidateLifetime = true,          // Habilita a valida��o do tempo de vida do token
        ValidateIssuerSigningKey = true,  // Habilita a valida��o da chave de assinatura do emissor
        IssuerSigningKey = new SymmetricSecurityKey(key), // Define a chave de assinatura usada para validar o token
        ClockSkew = TimeSpan.Zero         // Remove a margem de erro padr�o para o tempo de expira��o do token
    };
});

// Adiciona servi�os ao cont�iner da aplica��o
builder.Services.AddControllers(); // Adiciona suporte para controllers

builder.Services.AddEndpointsApiExplorer(); // Adiciona o servi�o para gera��o autom�tica de endpoints para a documenta��o

// Configura��o do Swagger para gerar a documenta��o da API
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "APE WebAPI", Version = "v1.0" }); });

// Configura a conex�o com o banco de dados usando a string de conex�o obtida das vari�veis de ambiente ou do arquivo de configura��o
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_Ape")
    ?? builder.Configuration.GetConnectionString("Ape");

builder.Services.AddDbContext<DbApe>(options =>
    options.UseSqlServer(connectionString)); // Configura o DbContext para usar o SQL Server

// Configura��o do CORS (Cross-Origin Resource Sharing) para permitir requisi��es de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:3000",
                           "http://localhost:5288",
                           "https://ape-web.vercel.app",
                           "https://ape.azurewebsites.net",
                           "https://ape-dev.azurewebsites.net/")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Ativando o CORS usando a pol�tica definida anteriormente
app.UseCors("AllowSpecificOrigins");

// Configura o pipeline de requisi��es HTTP
app.UseSwagger(); // Habilita a gera��o da documenta��o Swagger
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API V1.0")); // Define o endpoint para a interface do Swagger

app.UseAuthentication(); // Habilita o middleware de autentica��o para validar tokens JWT
app.UseAuthorization();  // Habilita o middleware de autoriza��o

app.MapControllers(); // Mapeia os controladores para os endpoints definidos

app.Run(); // Executa a aplica��o