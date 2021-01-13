using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerCargaCotacoes.PageObjects;
using WorkerCargaCotacoes.Data;
using WorkerCargaCotacoes.Documents;

namespace WorkerCargaCotacoes
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly CotacoesRepository _repository;
        private readonly int _intervaloExecucao;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            CotacoesRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
            _intervaloExecucao = Convert.ToInt32(
                configuration["IntervaloExecucao"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Extracao iniciada em: {time}", DateTimeOffset.Now);

                _logger.LogInformation("Iniciando leitura da pagina de cotacoes...");
                var pagina = new PaginaCotacoes(_configuration);
                try
                {
                    pagina.CarregarPagina();
                    
                    var cotacoesMoedasEstrangeiras =
                        pagina.ObterCotacoesMoedasEstrangeiras();
                    _repository.Incluir<CotacaoMoedaEstrangeiraDocument>(
                        cotacoesMoedasEstrangeiras);
                    _logger.LogInformation(
                        "Cotacoes de moedas estrangeiras registradas com sucesso");
                    
                    var cotacaoBitcoin =
                        pagina.ObterCotacaoBitcoin();
                    _repository.Incluir<CotacaoBitcoinDocument>(cotacaoBitcoin);
                    _logger.LogInformation(
                        "Cotacoes de Bitcoin registrada com sucesso");
                }
                finally
                {
                    pagina.Fechar();
                }
                _logger.LogInformation("Leitura da pagina de cotacoes concluida");

                _logger.LogInformation("Extracao concluida em: {time}", DateTimeOffset.Now);
                await Task.Delay(_intervaloExecucao, stoppingToken);
            }
        }
    }
}