FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/Howestprime.Movies.Main/Howestprime.Movies.Main.csproj ./src/Howestprime.Movies.Main/
COPY src/Howestprime.Movies.Application/Howestprime.Movies.Application.csproj ./src/Howestprime.Movies.Application/
COPY src/Howestprime.Movies.Domain/Howestprime.Movies.Domain.csproj    ./src/Howestprime.Movies.Domain/
COPY src/Howestprime.Movies.Infrastructure/Howestprime.Movies.Infrastructure.csproj ./src/Howestprime.Movies.Infrastructure/
COPY tests/UnitTests/UnitTests.csproj                                 ./tests/UnitTests/
COPY Howestprime.Movies.sln .

RUN dotnet restore "Howestprime.Movies.sln"

COPY . .
WORKDIR "/src/src/Howestprime.Movies.Main"
RUN dotnet build "Howestprime.Movies.Main.csproj" \
    -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Howestprime.Movies.Main.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish \
    /p:UseAppHost=false --os linux --arch x64

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

COPY config/ /config/

ENTRYPOINT ["dotnet", "Howestprime.Movies.Main.dll"]
