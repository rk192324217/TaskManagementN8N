# ---------- Build Stage ----------
# Use the .NET SDK image to build your project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything into the container
COPY . .

# Restore NuGet packages
RUN dotnet restore "MyNotesApi.csproj"

# Publish the project in Release mode to /app/publish
RUN dotnet publish "MyNotesApi.csproj" -c Release -o /app/publish

# ---------- Runtime Stage ----------
# Use a smaller runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/publish .
# Expose Render's port
EXPOSE 8080
# Force ASP.NET Core to listen on the Render-assigned $PORT
ENV ASPNETCORE_URLS=http://+:8080
# Start the API
ENTRYPOINT ["dotnet", "TaskManagement.dll"]
