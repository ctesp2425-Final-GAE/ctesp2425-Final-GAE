# Use a imagem base .NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Diretório de trabalho no container
WORKDIR /app

# Copia o arquivo .csproj para o container
COPY RestaurantReservationAPI.csproj ./

# Restaura as dependências do projeto
RUN dotnet restore "./RestaurantReservationAPI.csproj"

# Copia todos os arquivos do projeto para o container
COPY . ./

# Realiza o publish para o diretório de saída como Release
RUN dotnet publish "./RestaurantReservationAPI.csproj" -c Release -o /out

# Imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Diretório de trabalho da imagem final
WORKDIR /app

# Copia os arquivos publicados da etapa anterior para a imagem final
COPY --from=build /out .

# Comando padrão para executar a aplicação
ENTRYPOINT ["dotnet", "RestaurantReservationAPI.dll"]