FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ConsultaVeiculo.API/ConsultaVeiculo.API.csproj ConsultaVeiculo.API/
RUN dotnet restore ConsultaVeiculo.API/ConsultaVeiculo.API.csproj
COPY . .
WORKDIR /src/ConsultaVeiculo.API
RUN dotnet build ConsultaVeiculo.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ConsultaVeiculo.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConsultaVeiculo.API.dll"]
