# Consulta Veículo - API

API em ASP.NET Core para consulta de informações de veículos utilizando robô através do [Selenium Remote](https://www.seleniumhq.org/projects/remote-control/).

## Atualização dos dados

Os dados disponibilizados pela API são apenas de veículos pertencentes ao [DETRAN - DF](http://www.detran.df.gov.br/).

## Retorno - JSON

```json
 {
    "Renavam": "string",
    "Chassi": "string",
    "MarcaModelo": "string",
    "Cor": "string",
    "AnoModelo": "string",
    "Tipo": "string",
    "PotenciaCilindradas": "string",
    "Categoria": "string",
    "CapacidadePassageiros": "string",
    "Especie": "string",
    "Nacionalidade": "string",
    "Municipio": "string",
    "RouboFurto": "string",
    "SituacaoVeiculo": "string",
    "AnoUltimoLicenciamento": "string",
    "Restricao": "string",
    "Multa": "string"
 }
```

## Instruções para execução do projeto

**Executar Selenium Standalone - Docker**:
 - `docker run -d -p 4444:4444 selenium/standalone-chrome:3.4.0`
 - `http://localhost:4444/grid/console`
 
 **Executar API**
 - Executar projeto via docker ou via Visual Studio ou Visual Studio Code 
