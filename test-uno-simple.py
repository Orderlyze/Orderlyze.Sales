#!/usr/bin/env python3
"""
Simple test to check UnoApp API calls
"""

import urllib.request
import json
import time

def test_uno_app():
    print("Testing UnoApp functionality...")
    
    # Test 1: Check if UnoApp is running
    try:
        response = urllib.request.urlopen('http://localhost:5000')
        print(f"✓ UnoApp is running (status: {response.status})")
    except Exception as e:
        print(f"✗ UnoApp not accessible: {e}")
        return
    
    # Test 2: Check if WebAPI is running
    try:
        response = urllib.request.urlopen('http://localhost:5062/openapi/v1.json')
        print(f"✓ WebAPI is running (status: {response.status})")
    except Exception as e:
        print(f"✗ WebAPI not accessible: {e}")
        return
    
    # Test 3: Try login via API directly
    print("\nTesting login endpoint directly...")
    login_data = json.dumps({
        "email": "test@test.at",
        "password": "Test1234!"
    }).encode('utf-8')
    
    req = urllib.request.Request(
        'http://localhost:5062/login',
        data=login_data,
        headers={'Content-Type': 'application/json'}
    )
    
    try:
        response = urllib.request.urlopen(req)
        result = json.loads(response.read().decode('utf-8'))
        print("✓ Login successful via API!")
        print(f"  - Access token: {result['accessToken'][:50]}...")
        print(f"  - Token type: {result['tokenType']}")
        print(f"  - Expires in: {result['expiresIn']} seconds")
    except urllib.error.HTTPError as e:
        print(f"✗ Login failed: {e.code} {e.reason}")
        error_body = e.read().decode('utf-8')
        print(f"  Error details: {error_body}")
    except Exception as e:
        print(f"✗ Login error: {e}")
    
    # Test 4: Check what endpoints the UnoApp might be calling
    print("\nChecking potential UnoApp endpoints...")
    endpoints = [
        'http://localhost:5062/api/login',
        'http://localhost:5062/identity/login',
        'http://localhost:5062/auth/login',
        'http://localhost:5001/login',
        'https://localhost:5001/login'
    ]
    
    for endpoint in endpoints:
        try:
            req = urllib.request.Request(endpoint, method='POST')
            response = urllib.request.urlopen(req, timeout=2)
            print(f"  Found endpoint: {endpoint}")
        except:
            pass

if __name__ == "__main__":
    test_uno_app()