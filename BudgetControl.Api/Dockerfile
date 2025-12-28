# -------- BUILD --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# -------- RUNTIME --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Porta usada pelo Fly
ENV ASPNETCORE_URLS=http://+:8080

# Copia o app publicado
COPY --from=build /app/publish .

# Cria diretório do SQLite (volume será montado aqui)
RUN mkdir -p /data

EXPOSE 8080

ENTRYPOINT ["dotnet", "BudgetControl.Api.dll"]
