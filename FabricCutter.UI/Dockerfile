FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY . .
RUN dotnet restore FabricCutter.UI.csproj
COPY . .
RUN dotnet build FabricCutter.UI.csproj -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish FabricCutter.UI.csproj -c Release -o /app/publish
 
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY --from=publish /app/publish/nginx.conf /etc/nginx/nginx.conf

# docker build -t fbicsharp/FabricCutter.UI:V0.1 .
# docker run -p 34501:80 fbicsharp/FabricCutter.UI:V0.1
# docker push fbicsharp/FabricCutter.UI:V0.1
