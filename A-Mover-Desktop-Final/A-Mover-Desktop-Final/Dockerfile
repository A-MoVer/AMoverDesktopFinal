# Imagem base para construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos de projeto e restaurar dependências
COPY ["A-Mover-Desktop-Final.csproj", "./"]
RUN dotnet restore "A-Mover-Desktop-Final.csproj"

# Copiar todo o código-fonte
COPY . .

# Compilar o aplicativo
RUN dotnet build "A-Mover-Desktop-Final.csproj" -c Release -o /app/build

# Publicar o aplicativo
FROM build AS publish
RUN dotnet publish "A-Mover-Desktop-Final.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configurar variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expor a porta 80
EXPOSE 80

# Comando para executar o aplicativo
ENTRYPOINT ["dotnet", "A-Mover-Desktop-Final.dll"]