using ConsultaVeiculo.Core.Enum;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Remote;

namespace ConsultaVeiculo.Core.Configuration
{
    public class BrowserConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly Browser _browser;

        public BrowserConfiguration(IConfiguration configuration, Browser browser)
        {
            _configuration = configuration;
            _browser = browser;
        }

        public string DriverBrowser()
        {
            string driver = null;

            if (_browser == Browser.Chrome)
            {
                driver = _configuration.GetSection("Driver:Chrome").Value;
            }
            else if (_browser == Browser.Firefox)
            {
                driver = _configuration.GetSection("Driver:Firefox").Value;
            }
            else
            {
                driver = _configuration.GetSection("Driver:Chrome").Value;
            }

            return driver;
        }

        public DesiredCapabilities RemoteBrowser()
        {
            DesiredCapabilities driver = null;

            if (_browser == Browser.Chrome)
            {
                driver = DesiredCapabilities.Chrome();
            }
            else if (_browser == Browser.Firefox)
            {
                driver = DesiredCapabilities.Firefox();
            }
            else
            {
                driver = DesiredCapabilities.Chrome();
            }

            return driver;
        }
    }
}
