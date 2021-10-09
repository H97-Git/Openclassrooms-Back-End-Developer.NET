#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CalifornianHealth.Demographics/CalifornianHealth.Demographics.csproj", "CalifornianHealth.Demographics/"]
RUN dotnet restore "CalifornianHealth.Demographics/CalifornianHealth.Demographics.csproj"
COPY . .
WORKDIR "/src/CalifornianHealth.Demographics"
RUN dotnet build "CalifornianHealth.Demographics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalifornianHealth.Demographics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalifornianHealth.Demographics.dll"]
