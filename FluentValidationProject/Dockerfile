FROM mcr.microsoft.com/dotnet/aspnet:5.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["FluentValidationProject/FluentValidationProject.csproj", "FluentValidationProject/"]
RUN dotnet restore "FluentValidationProject\FluentValidationProject.csproj"
COPY . .
WORKDIR "/src/FluentValidationProject"
RUN dotnet build "FluentValidationProject.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "FluentValidationProject.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluentValidationProject.dll"]
