# Wix API Configuration

## Setting up your Wix API Key

For security reasons, the Wix API key should never be committed to source control and MUST be provided via environment variables.

### Option 1: Environment Variables (Recommended for Production)

Set the environment variable:
```bash
# Linux/Mac
export WixApi__ApiKey="YOUR_API_KEY_HERE"

# Windows Command Prompt
set WixApi__ApiKey=YOUR_API_KEY_HERE

# Windows PowerShell
$env:WixApi__ApiKey="YOUR_API_KEY_HERE"
```

### Option 2: User Secrets (Recommended for Development)

1. Navigate to the UnoApp project directory:
   ```bash
   cd src/UnoApp/UnoApp
   ```

2. Initialize user secrets:
   ```bash
   dotnet user-secrets init
   ```

3. Set the API key:
   ```bash
   dotnet user-secrets set "WixApi:ApiKey" "YOUR_API_KEY_HERE"
   ```

### Option 3: .env File (Local Development Only)

1. The `.env` file in `src/UnoApp/UnoApp/.env` will be automatically loaded
2. This file is already in `.gitignore` and contains the API key
3. The environment variable will be loaded at startup

### Option 4: Azure Key Vault or other Secret Management Services

For production environments, consider using:
- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault

## Configuration Priority

The API key is ONLY loaded from environment variables for security reasons:
1. Environment variable `WixApi__ApiKey` (double underscore)
2. Environment variable `WIXAPI__APIKEY` (all caps fallback)
3. `.env` file (automatically loaded in development)

The API key is NOT loaded from appsettings.json or appsettings.Development.json.

## Verifying Configuration

To verify your API key is properly configured, check the `WixApi:ApiKey` configuration value at runtime.