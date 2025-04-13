FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Flashcards.KamilKolanowski.csproj", "./"]
RUN dotnet restore "Flashcards.KamilKolanowski.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Flashcards.KamilKolanowski.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Flashcards.KamilKolanowski.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Flashcards.KamilKolanowski.dll"]
