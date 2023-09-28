FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ZeroVolumeFader.Service/ZeroVolumeFader.Service.csproj", "ZeroVolumeFader.Service/"]
RUN dotnet restore "ZeroVolumeFader.Service/ZeroVolumeFader.Service.csproj"
COPY . .
WORKDIR "/src/ZeroVolumeFader.Service"
RUN dotnet build "ZeroVolumeFader.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZeroVolumeFader.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZeroVolumeFader.Service.dll"]
