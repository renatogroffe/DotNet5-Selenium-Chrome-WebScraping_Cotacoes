using System.Collections.Generic;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace WorkerCargaCotacoes.Data
{
    public class CotacoesRepository
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly string _collection;

        public CotacoesRepository(IConfiguration configuration)
        {
            _client = new MongoClient(
                configuration["MongoDB:ConnectionString"]);
            _db = _client.GetDatabase(configuration["MongoDB:Database"]);
            _collection = configuration["MongoDB:Collection"];
        }

        public void Incluir<T>(T dadosCotacao)
        {
            var cotacoes = _db.GetCollection<T>(_collection);
            cotacoes.InsertOne(dadosCotacao);
        }

        public void Incluir<T>(IEnumerable<T> dadosCotacoes)
        {
            var cotacoes = _db.GetCollection<T>(_collection);
            cotacoes.InsertMany(dadosCotacoes);
        }
    }
}