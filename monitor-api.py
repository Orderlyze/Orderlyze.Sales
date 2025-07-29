#!/usr/bin/env python3
"""
Monitor API calls in real-time
"""

import subprocess
import time
import sys

print("Monitoring API logs...")
print("Press Ctrl+C to stop\n")

# Start monitoring the API process output
try:
    # Run the API and capture output
    proc = subprocess.Popen(
        ['dotnet', 'run', '--project', 'src/WebApi/WebApi.csproj'],
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        universal_newlines=True,
        bufsize=1
    )
    
    # Monitor output in real-time
    for line in iter(proc.stdout.readline, ''):
        if line:
            # Highlight login-related messages
            if any(keyword in line.lower() for keyword in ['login', 'auth', 'cors', 'request']):
                print(f">>> {line.strip()}")
            else:
                print(line.strip())
            sys.stdout.flush()
            
except KeyboardInterrupt:
    print("\nStopping monitoring...")
    proc.terminate()