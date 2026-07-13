FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedKernel/Bookshop.SharedKernel/Bookshop.SharedKernel.csproj SharedKernel/Bookshop.SharedKernel/
COPY SharedKernel/Bookshop.Contracts/Bookshop.Contracts.csproj SharedKernel/Bookshop.Contracts/
COPY SharedKernel/Bookshop.Contracts.Grpc/Bookshop.Contracts.Grpc.csproj SharedKernel/Bookshop.Contracts.Grpc/
COPY Basket/Basket.Domain/Basket.Domain.csproj Basket/Basket.Domain/
COPY Basket/Basket.Application/Basket.Application.csproj Basket/Basket.Application/
COPY Basket/Basket.Infrastructure/Basket.Infrastructure.csproj Basket/Basket.Infrastructure/
COPY Basket/Basket.API/Basket.API.csproj Basket/Basket.API/

RUN dotnet restore "Basket/Basket.API/Basket.API.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "Basket/Basket.API/Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]