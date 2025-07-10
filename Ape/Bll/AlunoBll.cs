using System;
using MongoDB.Driver;
using Ape.Entity;

namespace Ape.Bll
{
    public class AlunoBll
    {
        private readonly IMongoCollection<Aluno> database;
        private readonly HttpClient client;

        public AlunoBll(IMongoCollection<Aluno> _database)
        {
            database = _database;
            client = new HttpClient();
        }

        public Aluno PesquisarAplicacao(AlunoDto alunoDto)
        {
            try
            {
                Aluno aluno = new Aluno();
                aluno = database.Find(u => u.AppId == alunoDto.AppId).FirstOrDefault();
                return aluno;
            }
            catch
            {
                throw new ArgumentException("Aplicação não identificada");
            }
        }
    }
}