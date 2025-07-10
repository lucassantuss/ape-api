using System;
using Ape.Entity;
using MongoDB.Driver;

namespace Ape.Bll
{
    public class PersonalBll
    {
        private readonly IMongoCollection<Personal> database;
        private readonly HttpClient client;

        public PersonalBll(IMongoCollection<Personal> _database)
        {
            database = _database;
            client = new HttpClient();
        }
    }
}