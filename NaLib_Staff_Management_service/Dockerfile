#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NaLib_Staff_Management_service/NaLib_Staff_Management_service.csproj", "NaLib_Staff_Management_service/"]
RUN dotnet restore "./NaLib_Staff_Management_service/NaLib_Staff_Management_service.csproj"
COPY . .
WORKDIR "/src/NaLib_Staff_Management_service"
RUN dotnet build "./NaLib_Staff_Management_service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NaLib_Staff_Management_service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NaLib_Staff_Management_service.dll"]