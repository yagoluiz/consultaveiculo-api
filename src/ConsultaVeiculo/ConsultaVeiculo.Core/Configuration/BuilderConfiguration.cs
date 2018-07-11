using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConsultaVeiculo.Core.Configuration
{
    public static class BuilderConfiguration
    {
        public static IConfigurationRoot AddConfigurationDirectory()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}