FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["StudentMarksManagementSystem.csproj", "./"]
RUN dotnet restore "StudentMarksManagementSystem.csproj"

COPY . .
RUN dotnet publish "StudentMarksManagementSystem.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StudentMarksManagementSystem.dll"]
