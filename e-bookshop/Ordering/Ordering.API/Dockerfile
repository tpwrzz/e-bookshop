FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedKernel/Bookshop.SharedKernel/Bookshop.SharedKernel.csproj SharedKernel/Bookshop.SharedKernel/
COPY Ordering/Ordering.Domain/Ordering.Domain.csproj Ordering/Ordering.Domain/
COPY Ordering/Ordering.Application/Ordering.Application.csproj Ordering/Ordering.Application/
COPY Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj Ordering/Ordering.Infrastructure/
COPY Ordering/Ordering.API/Ordering.API.csproj Ordering/Ordering.API/

RUN dotnet restore "Ordering/Ordering.API/Ordering.API.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "Ordering/Ordering.API/Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]