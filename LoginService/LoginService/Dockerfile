FROM mcr.microsoft.com/dotnet/sdk:6.0

COPY bin/net5.0/publish . 

EXPOSE 13004

ENTRYPOINT ["dotnet", "LoginService.dll"]
