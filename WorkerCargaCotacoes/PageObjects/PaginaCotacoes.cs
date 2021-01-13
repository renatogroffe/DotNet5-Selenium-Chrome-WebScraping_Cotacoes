using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WorkerCargaCotacoes.Documents;

namespace WorkerCargaCotacoes.PageObjects
{
    public class PaginaCotacoes
    {
        private readonly IConfiguration _configuration;
        private IWebDriver _driver;

        public PaginaCotacoes(IConfiguration configuration)
        {
            _configuration = configuration;

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            _driver = new ChromeDriver(
                _configuration["CaminhoChromeDriver"],
                chromeOptions);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl(
                _configuration["UrlPaginaCotacoes"]);
        }

        public List<CotacaoMoedaEstrangeiraDocument> ObterCotacoesMoedasEstrangeiras()
        {
            var cotacoes = new List<CotacaoMoedaEstrangeiraDocument>();

            var rowsCotacoes = _driver
                .FindElement(By.Id("tableCotacoes"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"));
            foreach (var rowCotacao in rowsCotacoes)
            {
                var dadosCotacao = rowCotacao.FindElements(
                    By.TagName("td"));

                var cotacao = new CotacaoMoedaEstrangeiraDocument();
                cotacao.Codigo = dadosCotacao[0].Text;
                cotacao.NomeMoeda = dadosCotacao[1].Text;
                cotacao.Variacao = dadosCotacao[2].Text;
                cotacao.ValorReais = Convert.ToDouble(
                    dadosCotacao[3].Text);

                cotacoes.Add(cotacao);
            }

            return cotacoes;
        }

        public CotacaoBitcoinDocument ObterCotacaoBitcoin()
        {
            CotacaoBitcoinDocument cotacao = null;

            var cotacaoBitcoinHTML = _driver.FindElement(
                By.Id("cotacaoBitcoin"));
            if (cotacaoBitcoinHTML != null)
            {
                cotacao = new CotacaoBitcoinDocument();
                cotacao.NomeMoeda = "BITCOIN";
                cotacao.UltimaAtualizacao = DateTime.Now;
                cotacao.VlCotacaoDolar = Convert.ToDouble(
                    cotacaoBitcoinHTML.Text);
            }

            return cotacao;
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}