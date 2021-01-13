using System;
using MongoDB.Bson;

namespace WorkerCargaCotacoes.Documents
{
    public class CotacaoBitcoinDocument
    {
        public ObjectId _id { get; set; }
        public string NomeMoeda { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public double VlCotacaoDolar { get; set; }
    }
}