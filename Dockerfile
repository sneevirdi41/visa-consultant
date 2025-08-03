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

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "visa_consulatant.dll"] 