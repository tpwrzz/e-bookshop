FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedKernel/Bookshop.SharedKernel/Bookshop.SharedKernel.csproj SharedKernel/Bookshop.SharedKernel/
COPY SharedKernel/Bookshop.Contracts.Grpc/Bookshop.Contracts.Grpc.csproj SharedKernel/Bookshop.Contracts.Grpc/
COPY Catalog/Catalog.Domain/Catalog.Domain.csproj Catalog/Catalog.Domain/
COPY Catalog/Catalog.Application/Catalog.Application.csproj Catalog/Catalog.Application/
COPY Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj Catalog/Catalog.Infrastructure/
COPY Catalog/Catalog.API/Catalog.API.csproj Catalog/Catalog.API/

RUN dotnet restore "Catalog/Catalog.API/Catalog.API.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "Catalog/Catalog.API/Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]