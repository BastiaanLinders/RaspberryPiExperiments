FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HelloWorldFinal/HelloWorldFinal.csproj", "HelloWorldFinal/"]
RUN dotnet restore "HelloWorldFinal/HelloWorldFinal.csproj"
COPY . .
WORKDIR "/src/HelloWorldFinal"
RUN dotnet build "HelloWorldFinal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloWorldFinal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloWorldFinal.dll"]