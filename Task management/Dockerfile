# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only project file first (to leverage caching)
COPY "Task management/MyNotesApi.csproj" "Task management/"

# Restore NuGet packages
RUN dotnet restore "Task management/MyNotesApi.csproj"

# Copy everything else
COPY . .

# Go into project folder
WORKDIR "/src/Task management"

# Publish in Release mode
RUN dotnet publish "MyNotesApi.csproj" -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Run the API
ENTRYPOINT ["dotnet", "MyNotesApi.dll"]
