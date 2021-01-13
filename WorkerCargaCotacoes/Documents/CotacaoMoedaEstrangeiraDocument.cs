using MongoDB.Bson;

namespace WorkerCargaCotacoes.Documents
{
    public class CotacaoMoedaEstrangeiraDocument
    {
        public ObjectId _id { get; set; }
        public string Codigo { get; set; }
        public string NomeMoeda { get; set; }
        public string Variacao { get; set; }
        public double ValorReais { get; set; }
    }
}