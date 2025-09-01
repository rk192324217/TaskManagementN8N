# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything into the container
COPY . .

# Go inside Task management folder to restore and build
WORKDIR "/src/Task management"

# Restore NuGet packages
RUN dotnet restore "MyNotes.csproj"

# Publish the project in Release mode
RUN dotnet publish "MyNotes.csproj" -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published app
COPY --from=build /app/publish .

# Start the API
ENTRYPOINT ["dotnet", "MyNotes.dll"]
