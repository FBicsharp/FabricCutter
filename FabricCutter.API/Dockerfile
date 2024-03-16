FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
EXPOSE 33500
EXPOSE 33501
EXPOSE 80

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out



FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
RUN chmod 755 /App/
ENTRYPOINT ["dotnet","FabricCutter.API.dll"]

# docker build -t fbicsharp/FabricCutter.API:V0.1 .
# docker run -p 5000:80 fbicsharp/FabricCutter.API:V0.1
# docker push fbicsharp/FabricCutter.API:V0.1
