using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ape.Database;
using System.Text;
using MongoDB.Driver;
using Ape.Entity;
using Ape.Bll;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configurações do JWT - Define a chave secreta para o token JWT, que pode ser obtida das variáveis de ambiente ou do arquivo de configuração.
var jwtSecretKey = Environment.GetEnvironmentVariable("Jwt_SecretKey")
    ?? builder.Configuration["Jwt_SecretKey"];

// Verifica se a chave secreta do JWT está definida; se não estiver, lança uma exceção.
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("A chave secreta JWT não está definida.");
}

// Converte a chave secreta para um array de bytes para ser utilizada na configuração do token.
var key = Encoding.ASCII.GetBytes(jwtSecretKey);

// Configura o serviço de autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema padrão para autenticação
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // Define o esquema padrão para desafios de autenticação
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Desativa o requisito de HTTPS para obter tokens (útil para desenvolvimento)
    options.SaveToken = true;             // Salva o token JWT no contexto da autenticação
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,           // Desabilita a validação do emissor do token
        ValidateAudience = false,         // Desabilita a validação da audiência do token
        ValidateLifetime = true,          // Habilita a validação do tempo de vida do token
        ValidateIssuerSigningKey = true,  // Habilita a validação da chave de assinatura do emissor
        IssuerSigningKey = new SymmetricSecurityKey(key), // Define a chave de assinatura usada para validar o token
        ClockSkew = TimeSpan.Zero         // Remove a margem de erro padrão para o tempo de expiração do token
    };
});

// Adiciona serviços ao container da aplicação
builder.Services.AddControllers(); // Adiciona suporte para controllers

builder.Services.AddEndpointsApiExplorer(); // Adiciona o serviço para geração autom�tica de endpoints para a documenta��o

// Configuração do Swagger para gerar a documentação da API
builder.Services.AddSwaggerGen(c =>
{
    // Cria um documento Swagger (OpenAPI) com informações básicas da API
    c.SwaggerDoc("v1.0",
        new OpenApiInfo
        {
            Title = "APE WebAPI", // Título da API que aparecerá na UI do Swagger
            Description = "API utilizada no sistema APE", // Descrição da API
            Version = "v1.0" // Versão da API
        }
    );

    // Configura o esquema de segurança para autenticação via JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", // Nome do cabeçalho onde o token será enviado
        Type = SecuritySchemeType.ApiKey, // Tipo ApiKey indica que será passado via header
        Scheme = "Bearer", // Esquema de autenticação: Bearer Token
        BearerFormat = "JWT", // Formato esperado do token
        In = ParameterLocation.Header, // Define que o token virá no cabeçalho da requisição
        Description = "Insira o token JWT no campo abaixo:\n\nExemplo: Bearer {seu token}" // Instrução que aparecerá no Swagger UI
    });

    // Exige que todas as operações utilizem o esquema de segurança configurado acima
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // Informa que é um esquema de segurança definido
                    Id = "Bearer" // Nome do esquema (mesmo definido acima)
                }
            },
            Array.Empty<string>() // Escopos (não utilizados aqui, pois JWT já contém as permissões)
        }
    });

    // Carrega automaticamente os comentários XML do código (summary, param, returns etc.)
    // Isso gera uma documentação mais detalhada no Swagger UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Nome do arquivo XML baseado no nome do assembly
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // Caminho completo até o arquivo XML
    c.IncludeXmlComments(xmlPath); // Inclui os comentários XML na documentação do Swagger
});

// Configura a conexão com o banco de dados usando a string de conexão obtida das variáveis de ambiente ou do arquivo de configuração
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_Ape")
    ?? builder.Configuration.GetConnectionString("Ape");

// builder.Services.AddDbContext<DbApe>(options => options.UseSqlServer(connectionString)); // Configura o DbContext para usar o SQL Server

// Adiciona o user-secrets apenas em ambiente de DESENVOLVIMENTO
// Comandos:
// dotnet user-secrets init
// dotnet user-secrets set "MongoDB:ConnectionString" "inserir-string-de-conexao-do-mongoDB"
// dotnet user-secrets set "MongoDB:Database" "inserir-db-mongoDB" 
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Carregar configurções do appsettings.json // variáveis de ambiente
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// MongoClient singleton
builder.Services.AddSingleton<MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Banco de dados
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = sp.GetRequiredService<MongoClient>();
    return client.GetDatabase(settings.Database);
});

// Injeção das Coleções
builder.Services.AddSingleton<IMongoCollection<Personal>>(sp =>
    sp.GetRequiredService<IMongoDatabase>().GetCollection<Personal>("collection_personal"));

builder.Services.AddSingleton<IMongoCollection<Aluno>>(sp =>
    sp.GetRequiredService<IMongoDatabase>().GetCollection<Aluno>("collection_aluno"));

builder.Services.AddSingleton<IMongoCollection<Exercicio>>(sp =>
    sp.GetRequiredService<IMongoDatabase>().GetCollection<Exercicio>("collection_exercicio"));

// Serviços
builder.Services.AddSingleton<AlunoBll>();
builder.Services.AddSingleton<ExercicioBll>();
builder.Services.AddSingleton<PersonalBll>();

// Configuração do CORS (Cross-Origin Resource Sharing) para permitir requisições de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:3000",
                           "http://localhost:5288",
                           "https://ape-web.vercel.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Ativando o CORS usando a política definida anteriormente
app.UseCors("AllowSpecificOrigins");

// Configura o pipeline de requisições HTTP
app.UseSwagger(); // Habilita a geração da documentação Swagger
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API V1.0")); // Define o endpoint para a interface do Swagger

app.UseAuthentication(); // Habilita o middleware de autenticação para validar tokens JWT
app.UseAuthorization();  // Habilita o middleware de autorização

app.MapControllers(); // Mapeia os controladores para os endpoints definidos

app.Run(); // Executa a aplicação