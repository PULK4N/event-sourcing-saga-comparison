# Use sdk 8.0 to publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire project and build the app
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the port that the application will run on
EXPOSE 5000

# Define the entry point for the container
ENTRYPOINT ["dotnet", "SagaOrchestrator.dll"]
