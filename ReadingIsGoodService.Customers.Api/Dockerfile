#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ReadingIsGoodService.Customers.Api/ReadingIsGoodService.Customers.Api.csproj", "ReadingIsGoodService.Customers.Api/"]
COPY ["ReadingIsGoodService.Common/ReadingIsGoodService.Common.csproj", "ReadingIsGoodService.Common/"]
COPY ["ReadingIsGoodService.Customers.Logic/ReadingIsGoodService.Customers.Logic.csproj", "ReadingIsGoodService.Customers.Logic/"]
COPY ["ReadingIsGoodService.Data/ReadingIsGoodService.Data.csproj", "ReadingIsGoodService.Data/"]
RUN dotnet restore "ReadingIsGoodService.Customers.Api/ReadingIsGoodService.Customers.Api.csproj"
COPY . .
WORKDIR "/src/ReadingIsGoodService.Customers.Api"
RUN dotnet build "ReadingIsGoodService.Customers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReadingIsGoodService.Customers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReadingIsGoodService.Customers.Api.dll"]