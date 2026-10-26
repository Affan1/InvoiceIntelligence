# Dockerfile for InvoiceIntelligence.Api (week 1).
# Build from the repo root:
#   docker build -f src/InvoiceIntelligence.Api/Dockerfile -t invoiceintelligence-api .

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy and restore. TODO: narrow to the projects the API needs once stable.
COPY . .
RUN dotnet restore src/InvoiceIntelligence.Api/InvoiceIntelligence.Api.csproj
RUN dotnet publish src/InvoiceIntelligence.Api/InvoiceIntelligence.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InvoiceIntelligence.Api.dll"]
