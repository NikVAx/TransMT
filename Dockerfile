FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TrackMS.WebAPI/TrackMS.WebAPI.csproj", "src/TrackMS.WebAPI/"]
COPY ["src/TrackMS.Data/TrackMS.Data.csproj", "src/TrackMS.Data/"]
COPY ["src/TrackMS.Domain/TrackMS.Domain.csproj", "src/TrackMS.Domain/"]
RUN dotnet restore "./src/TrackMS.WebAPI/TrackMS.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/TrackMS.WebAPI"
RUN dotnet build "./TrackMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrackMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrackMS.WebAPI.dll"]