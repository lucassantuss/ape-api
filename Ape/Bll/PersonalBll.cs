using System;
using Ape.Entity;
using MongoDB.Driver;

namespace Ape.Bll
{
    public class PersonalBll
    {
        private readonly IMongoCollection<Personal> _database;
        private readonly HttpClient _client;

        public PersonalBll(IMongoCollection<Personal> database)
        {
            _database = database;
            _client = new HttpClient();
        }

        public Personal PesquisarPersonal(PersonalDto personalDto)
        {
            try
            {
                Personal personal = new Personal();
                personal = _database.Find(f => f.Usuario == personal.Usuario).FirstOrDefault();
                return personal;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }
    }
}