FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["./example-console/Example.csproj", ""]
RUN dotnet restore "Example.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./example-console/Example.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "./example-console/Example.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Example.dll"]
