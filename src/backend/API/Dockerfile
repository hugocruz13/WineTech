ARG DOTNET_SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:10.0
ARG DOTNET_ASPNET_IMAGE=mcr.microsoft.com/dotnet/aspnet:10.0

FROM ${DOTNET_SDK_IMAGE} AS build
WORKDIR /src

COPY . .
RUN dotnet restore API/API.csproj
RUN dotnet publish API/API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM ${DOTNET_ASPNET_IMAGE} AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "API.dll"]
