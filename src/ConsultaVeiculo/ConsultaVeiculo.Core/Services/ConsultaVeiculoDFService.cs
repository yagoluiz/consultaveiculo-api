using ConsultaVeiculo.Core.Configuration;
using ConsultaVeiculo.Core.Enum;
using ConsultaVeiculo.Core.Extensions;
using ConsultaVeiculo.Core.Factory;
using ConsultaVeiculo.Core.Models;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace ConsultaVeiculo.Core.Services
{
    public class ConsultaVeiculoDFService
    {
        private readonly IConfiguration _configuration;
        private IWebDriver _webDriver;

        public ConsultaVeiculoDFService(IConfiguration configuration, Browser browser, bool remote = true)
        {
            _configuration = configuration;

            var browserConfiguration = new BrowserConfiguration(_configuration, browser);

            if (!remote)
            {
                var driverBrowser = browserConfiguration.DriverBrowser();

                _webDriver = WebDriverFactory.CreateWebDriver(browser, driverBrowser);
            }
            else
            {
                var remoteBrowser = browserConfiguration.RemoteBrowser();
                remoteBrowser.SetCapability("acceptSslCerts", true);

                _webDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), remoteBrowser);
            }
        }

        public void CarregarPagina()
        {
            _webDriver.LoadPage(_configuration.GetSection("Url:ConsultaVeiculoDF").Value);
        }

        public void PreencherPlaca(string placa)
        {
            _webDriver.SetText(By.Name("PLACA"), placa.ToString());
        }

        public void PreencherRenavam(string renavam)
        {
            _webDriver.SetText(By.Name("RENAVAM"), renavam.ToString());
        }

        public void ProcessarConsulta()
        {
            _webDriver.FindElement(By.CssSelector("a[onclick*=enviar]")).Click();
        }

        public string ObterCaptcha()
        {
            Thread.Sleep(1000);

            var pergunta = _webDriver.GetText(By.Id("pergunta"));

            string captcha = string.Empty;
            int numero1 = 0;
            int numero2 = 0;

            for (int i = 0; i < pergunta.Length; i++)
            {
                if (char.IsDigit(pergunta[i]))
                {
                    captcha += pergunta[i];
                }
            }

            numero1 = (int.Parse(captcha) / 10);
            numero2 = (int.Parse(captcha) % 10);

            return (numero1 + numero2).ToString();
        }

        public void PreencherCaptcha(string captcha)
        {
            _webDriver.SetText(By.Name("CODSEG"), captcha);
        }

        public void ProcessarCaptcha()
        {
            _webDriver.FindElement(By.CssSelector("a[onclick*=submit]")).Click();
        }

        public VeiculoModel ObterDadosVeiculo()
        {
            var veiculo = new VeiculoModel
            {
                Placa = _webDriver.FindElement(By.Id("Placa")).GetAttribute("value"),
                Renavam = _webDriver.FindElement(By.Id("Renavam")).GetAttribute("value"),
                Chassi = _webDriver.FindElement(By.Id("Chassi")).GetAttribute("value"),
                MarcaModelo = _webDriver.FindElement(By.Id("MarcaModelo")).GetAttribute("value"),
                Cor = _webDriver.FindElement(By.Id("Cor")).GetAttribute("value"),
                AnoModelo = _webDriver.FindElement(By.Id("AnoGabModelo")).GetAttribute("value"),
                Tipo = _webDriver.FindElement(By.Id("Tipo")).GetAttribute("value"),
                Combustivel = _webDriver.FindElement(By.Id("Combustivel")).GetAttribute("value"),
                PotenciaCilindradas = _webDriver.FindElement(By.Id("PotenciaCilindradas")).GetAttribute("value"),
                Categoria = _webDriver.FindElement(By.Id("Categoria")).GetAttribute("value"),
                CapacidadePassageiros = _webDriver.FindElement(By.Id("CapacidadePassageiros")).GetAttribute("value"),
                Especie = _webDriver.FindElement(By.Id("Especie")).GetAttribute("value"),
                Nacionalidade = _webDriver.FindElement(By.Id("Nacionalidade")).GetAttribute("value"),
                Municipio = _webDriver.FindElement(By.Id("Municipio")).GetAttribute("value"),
                RouboFurto = _webDriver.FindElement(By.Id("RouboFurto")).GetAttribute("value"),
                SituacaoVeiculo = _webDriver.FindElement(By.Id("SituacaoVeiculo")).GetAttribute("value"),
                AnoUltimoLicenciamento = _webDriver.FindElement(By.Id("AnoUltimoLicenciamento")).GetAttribute("value"),
                Restricao = _webDriver.FindElement(By.Id("restricao")).GetAttribute("value"),
                Multa = _webDriver.PageSource.Contains("Não existe(m) débito(s) de Multa até o presente momento.") ? "UFA! NÃO HÁ MULTA NO SISTEMA" : "OPS! HÁ MULTA NO SISTEMA"
            };

            return veiculo;
        }

        public void Fechar()
        {
            _webDriver.Quit();
            _webDriver = null;
        }
    }
}
