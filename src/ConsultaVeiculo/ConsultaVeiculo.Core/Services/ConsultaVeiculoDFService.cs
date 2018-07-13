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
            var veiculo = new VeiculoModel();

            var pageSource = _webDriver.PageSource;

            if (pageSource != null)
            {
                veiculo.Placa = _webDriver.FindElement(By.Id("Placa")).GetAttribute("value");
                veiculo.Renavam = _webDriver.FindElement(By.Id("Renavam")).GetAttribute("value");
                veiculo.Chassi = _webDriver.FindElement(By.Id("Chassi")).GetAttribute("value");
                veiculo.MarcaModelo = _webDriver.FindElement(By.Id("MarcaModelo")).GetAttribute("value");
                veiculo.Cor = _webDriver.FindElement(By.Id("Cor")).GetAttribute("value");
                veiculo.AnoModelo = _webDriver.FindElement(By.Id("AnoGabModelo")).GetAttribute("value");
                veiculo.Tipo = _webDriver.FindElement(By.Id("Tipo")).GetAttribute("value");
                veiculo.Combustivel = _webDriver.FindElement(By.Id("Combustivel")).GetAttribute("value");
                veiculo.PotenciaCilindradas = _webDriver.FindElement(By.Id("PotenciaCilindradas")).GetAttribute("value");
                veiculo.Categoria = _webDriver.FindElement(By.Id("Categoria")).GetAttribute("value");
                veiculo.CapacidadePassageiros = _webDriver.FindElement(By.Id("CapacidadePassageiros")).GetAttribute("value");
                veiculo.Especie = _webDriver.FindElement(By.Id("Especie")).GetAttribute("value");
                veiculo.Nacionalidade = _webDriver.FindElement(By.Id("Nacionalidade")).GetAttribute("value");
                veiculo.Municipio = _webDriver.FindElement(By.Id("Municipio")).GetAttribute("value");
                veiculo.RouboFurto = _webDriver.FindElement(By.Id("RouboFurto")).GetAttribute("value");
                veiculo.SituacaoVeiculo = _webDriver.FindElement(By.Id("SituacaoVeiculo")).GetAttribute("value");
                veiculo.AnoUltimoLicenciamento = _webDriver.FindElement(By.Id("AnoUltimoLicenciamento")).GetAttribute("value");
                veiculo.Restricao = _webDriver.FindElement(By.Id("restricao")).GetAttribute("value");
                veiculo.Multa = _webDriver.PageSource.Contains("Não existe(m) débito(s) de Multa até o presente momento.") ? "UFA! NÃO HÁ MULTA NO SISTEMA" : "OPS! HÁ MULTA NO SISTEMA";
            }

            return veiculo;
        }

        public void Fechar()
        {
            _webDriver.Quit();
            _webDriver = null;
        }
    }
}
