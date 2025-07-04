# Wix API Configuration

## Setting up your Wix API Key

For security reasons, the Wix API key should never be committed to source control. Here are the recommended ways to configure it:

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

### Option 3: appsettings.Development.json (Local Development Only)

1. Update `src/UnoApp/UnoApp/appsettings.Development.json`
2. Replace `YOUR_WIX_API_KEY_HERE` with your actual API key
3. **IMPORTANT**: Ensure `appsettings.Development.json` is in your `.gitignore`

### Option 4: Azure Key Vault or other Secret Management Services

For production environments, consider using:
- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault

## Configuration Priority

The configuration is loaded in this order (later sources override earlier ones):
1. appsettings.json
2. appsettings.{Environment}.json
3. Environment variables
4. User secrets (Development only)
5. Command line arguments

## Verifying Configuration

To verify your API key is properly configured, check the `WixApi:ApiKey` configuration value at runtime.