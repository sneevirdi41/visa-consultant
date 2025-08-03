FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["visa_consulatant.csproj", "./"]
RUN dotnet restore "visa_consulatant.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "visa_consulatant.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "visa_consulatant.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install Entity Framework tools
RUN dotnet tool install --global dotnet-ef

# Copy deployment script
COPY railway-deploy.sh /app/
RUN chmod +x /app/railway-deploy.sh

# Use the deployment script as entrypoint
ENTRYPOINT ["/app/railway-deploy.sh"] 