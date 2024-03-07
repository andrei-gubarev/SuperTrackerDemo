# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SuperTracker.PixelService/SuperTracker.PixelService.csproj"
WORKDIR "/src/SuperTracker.PixelService"
RUN dotnet build "SuperTracker.PixelService.csproj" -c Release -o /app/build
RUN dotnet publish "SuperTracker.PixelService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "SuperTracker.PixelService.dll"]