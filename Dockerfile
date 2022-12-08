FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR  /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY MultiWeb/MultiApp.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release

FROM build AS publish
RUN dotnet publish -c Release -o /publish

FROM base AS final
WORKDIR  //app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "MultiApp.dll"]
