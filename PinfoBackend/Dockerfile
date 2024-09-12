FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY PinfoBackend.csproj ./
RUN dotnet restore "PinfoBackend.csproj" --disable-parallel

# Copy all the source code and build the project
COPY . ./
RUN dotnet build "PinfoBackend.csproj" -c $BUILD_CONFIGURATION

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PinfoBackend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /m:1

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PinfoBackend.dll"]
