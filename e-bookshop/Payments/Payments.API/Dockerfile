FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedKernel/Bookshop.SharedKernel/Bookshop.SharedKernel.csproj SharedKernel/Bookshop.SharedKernel/
COPY SharedKernel/Bookshop.Contracts/Bookshop.Contracts.csproj SharedKernel/Bookshop.Contracts/
COPY Payments/Payments.Domain/Payments.Domain.csproj Payments/Payments.Domain/
COPY Payments/Payments.Application/Payments.Application.csproj Payments/Payments.Application/
COPY Payments/Payments.Infrastructure/Payments.Infrastructure.csproj Payments/Payments.Infrastructure/
COPY Payments/Payments.API/Payments.API.csproj Payments/Payments.API/

RUN dotnet restore "Payments/Payments.API/Payments.API.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "Payments/Payments.API/Payments.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.API.dll"]