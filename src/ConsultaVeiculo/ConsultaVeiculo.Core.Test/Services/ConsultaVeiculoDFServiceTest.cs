using ConsultaVeiculo.Core.Configuration;
using ConsultaVeiculo.Core.Enum;
using ConsultaVeiculo.Core.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ConsultaVeiculo.Core.Test.Services
{
    public class ConsultaVeiculoDFServiceTest
    {
        private readonly IConfiguration _configuration;

        public ConsultaVeiculoDFServiceTest()
        {
            _configuration = BuilderConfiguration.AddConfigurationDirectory();
        }

        private void SeleniumRemote(Browser browser, string placa, string renavam)
        {
            var consultaVeiculoDF = new ConsultaVeiculoDFService(_configuration, browser, remote: true);

            consultaVeiculoDF.CarregarPagina();

            consultaVeiculoDF.PreencherPlaca(placa);
            consultaVeiculoDF.PreencherRenavam(renavam);

            consultaVeiculoDF.ProcessarConsulta();

            var captcha = consultaVeiculoDF.ObterCaptcha();
            consultaVeiculoDF.PreencherCaptcha(captcha);
            consultaVeiculoDF.ProcessarCaptcha();

            var veiculo = consultaVeiculoDF.ObterDadosVeiculo();

            consultaVeiculoDF.Fechar();

            Assert.Equal(placa, veiculo.Placa);
            Assert.Equal(renavam, veiculo.Renavam);
        }

        private void SeleniumBrowser(Browser browser, string placa, string renavam)
        {
            var consultaVeiculoDF = new ConsultaVeiculoDFService(_configuration, browser, remote: false);

            consultaVeiculoDF.CarregarPagina();

            consultaVeiculoDF.PreencherPlaca(placa);
            consultaVeiculoDF.PreencherRenavam(renavam);

            consultaVeiculoDF.ProcessarConsulta();

            var captcha = consultaVeiculoDF.ObterCaptcha();
            consultaVeiculoDF.PreencherCaptcha(captcha);
            consultaVeiculoDF.ProcessarCaptcha();

            var veiculo = consultaVeiculoDF.ObterDadosVeiculo();

            consultaVeiculoDF.Fechar();

            Assert.Equal(placa, veiculo.Placa);
            Assert.Equal(renavam, veiculo.Renavam);
        }

        [Fact]
        [Trait("Category", "Remote")]
        public void ConsultaVeiculoDFService_RemoteChromeTest()
        {
            SeleniumRemote(Browser.Chrome, "PBF4510", "01141425766");
        }

        [Fact]
        [Trait("Category", "Browser")]
        public void ConsultaVeiculoDFService_BrowserChromeTest()
        {
            SeleniumBrowser(Browser.Chrome, "PBF4510", "01141425766");
        }
    }
}
