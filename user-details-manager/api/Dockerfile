FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserDetailsManagerApi.csproj", "."]
RUN dotnet restore "./UserDetailsManagerApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./UserDetailsManagerApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserDetailsManagerApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root

COPY --from=publish /app/publish .

COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh

USER app

ENTRYPOINT ["./entrypoint.sh"]
