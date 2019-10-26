FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
COPY /bin/Release/netcoreapp2.2/linux-x64/publish/ /app
WORKDIR /app

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Iot.WebAPI.dll"]