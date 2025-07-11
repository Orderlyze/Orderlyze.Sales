using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UnoApp.Services.Configuration;

public static class DotEnvLoader
{
    public static void Load()
    {
        // Skip file system operations in WebAssembly
        if (OperatingSystem.IsBrowser())
        {
            return;
        }
        
        var envFile = FindEnvFile();
        if (envFile != null)
        {
            LoadEnvironmentVariables(envFile);
        }
    }

    private static string? FindEnvFile()
    {
        var searchPaths = new[]
        {
            AppDomain.CurrentDomain.BaseDirectory,
            Directory.GetCurrentDirectory(),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."),
        };

        foreach (var basePath in searchPaths)
        {
            var envPath = Path.Combine(basePath, ".env");
            if (File.Exists(envPath))
            {
                return envPath;
            }
        }

        return null;
    }

    private static void LoadEnvironmentVariables(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"));

            foreach (var line in lines)
            {
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception but don't throw - environment variables might be set elsewhere
            Debug.WriteLine($"Warning: Failed to load .env file from {filePath}: {ex.Message}");
        }
    }
}