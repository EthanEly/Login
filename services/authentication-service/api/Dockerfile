FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_HTTP_PORTS=8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthenticationServiceApi.csproj", "."]
RUN dotnet restore "./AuthenticationServiceApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./AuthenticationServiceApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthenticationServiceApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root

COPY --from=publish /app/publish .

COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh

USER app

ENTRYPOINT ["./entrypoint.sh"]
