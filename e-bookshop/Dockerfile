FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Gateway/BookShop.Gateway/BookShop.Gateway.csproj Gateway/BookShop.Gateway/

RUN dotnet restore "Gateway/Bookshop.Gateway/Bookshop.Gateway.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "Gateway/Bookshop.Gateway/Bookshop.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookshop.Gateway.dll"]