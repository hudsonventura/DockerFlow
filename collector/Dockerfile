FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
#RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
#USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG configuration=Release
WORKDIR /src

COPY ["./Shared/Shared.csproj", "Shared/"]
#RUN dotnet restore "Shared/Shared.csproj"

COPY ["./collector/collector.csproj", "collector/"]
RUN dotnet restore "collector/collector.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "collector/collector.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "collector/collector.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt update && apt install -y curl # to call the docker API
ENTRYPOINT ["dotnet", "collector.dll"]
