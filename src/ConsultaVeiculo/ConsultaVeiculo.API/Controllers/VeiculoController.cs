using ConsultaVeiculo.Core.Configuration;
using ConsultaVeiculo.Core.Enum;
using ConsultaVeiculo.Core.Models;
using ConsultaVeiculo.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace ConsultaVeiculo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/veiculos")]
    public class VeiculoController : Controller
    {
        private readonly IConfiguration _configuration;

        public VeiculoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Dados de veículo do Distrito Federal (DF)
        /// </summary>
        /// <param name="placa">Parâmetro da placa no formato "AAA9999".</param>
        /// <param name="renavam">Parâmetro do renavam "yyyyMM".</param>
        /// <returns>Dados de veículo do Distrito Federal (DF).</returns>
        /// <response code="200">Dados de veículo do Distrito Federal (DF).</response>
        /// <response code="400">Formato dos parâmetros inválidos.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpGet]
        [Route("df/{placa}/{renavam}")]
        [ProducesResponseType(typeof(VeiculoModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Get(string placa, string renavam)
        {
            if (!(Regex.IsMatch(placa, "^[a-zA-Z]{3}[0-9]{4}$") && Regex.IsMatch(renavam, "^([0-9]{11})$")))
            {
                return BadRequest("Formato dos parâmetros inválidos");
            }

            var consultaVeiculoDF = new ConsultaVeiculoDFService(BuilderConfiguration.AddConfigurationDirectory(), Browser.Chrome, remote: true);

            consultaVeiculoDF.CarregarPagina();

            consultaVeiculoDF.PreencherPlaca(placa);
            consultaVeiculoDF.PreencherRenavam(renavam);
            consultaVeiculoDF.ProcessarConsulta();

            var captcha = consultaVeiculoDF.ObterCaptcha();
            consultaVeiculoDF.PreencherCaptcha(captcha);
            consultaVeiculoDF.ProcessarCaptcha();

            var veiculo = consultaVeiculoDF.ObterDadosVeiculo();

            consultaVeiculoDF.Fechar();

            return Ok(veiculo);
        }
    }
}