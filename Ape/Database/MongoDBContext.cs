using Ape.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ape.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
        public IMongoCollection<Perfil> Perfis => _database.GetCollection<Perfil>("Perfis");
        public IMongoCollection<UsuarioPerfil> UsuarioPerfis => _database.GetCollection<UsuarioPerfil>("UsuarioPerfis");
        public IMongoCollection<Personal> Personal => _database.GetCollection<Personal>("collection_personal");
        public IMongoCollection<Aluno> Aluno => _database.GetCollection<Aluno>("collection_aluno");
        public IMongoCollection<Exercicio> Exercicio => _database.GetCollection<Exercicio>("collection_exercicio");
    }
}
