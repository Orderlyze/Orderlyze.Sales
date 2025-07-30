#!/bin/bash

# Orderlyze.Sales Deployment Script
# This script builds and prepares the application for deployment

set -e  # Exit on error

echo "Starting deployment process..."

# Build WebApi
echo "Building WebApi..."
dotnet build src/WebApi/WebApi.csproj -c Release

# Build UnoApp for WebAssembly
echo "Building UnoApp for WebAssembly..."
dotnet build src/UnoApp/UnoApp/UnoApp.csproj -c Release --framework net9.0-browserwasm

# Publish WebApi
echo "Publishing WebApi..."
dotnet publish src/WebApi/WebApi.csproj -c Release -o ./publish/webapi

# Publish UnoApp
echo "Publishing UnoApp..."
dotnet publish src/UnoApp/UnoApp/UnoApp.csproj -c Release --framework net9.0-browserwasm -o ./publish/unoapp

# Copy WebApi.json to UnoApp (ensure latest version)
echo "Copying WebApi.json..."
cp src/WebApi/WebApi.json src/UnoApp/UnoApp/WebApi.json

# Create deployment information
echo "Creating deployment info..."
cat > ./publish/deployment-info.txt << EOF
Deployment Date: $(date)
WebApi Version: $(git rev-parse --short HEAD)
Branch: $(git branch --show-current)

IMPORTANT: 
1. Set the WixApi:ApiKey environment variable on the server
2. Update appsettings.Production.json with correct connection strings
3. Ensure CORS is properly configured for production URLs
4. Update UnoApp ApiClient URL in appsettings.json to point to production WebApi
EOF

echo "Deployment preparation complete!"
echo "Published files are in ./publish directory"
echo ""
echo "Next steps:"
echo "1. Set WixApi:ApiKey environment variable on the server"
echo "2. Deploy ./publish/webapi to your API server"
echo "3. Deploy ./publish/unoapp to your web server"
echo "4. Update configuration files as needed"