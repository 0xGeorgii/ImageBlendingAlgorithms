FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["WebUI/WebUI.csproj", "WebUI/"]
RUN dotnet restore "WebUI/WebUI.csproj"
COPY . .
WORKDIR "/src/WebUI"
RUN dotnet build "WebUI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebUI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebUI.dll"]