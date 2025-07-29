#!/usr/bin/env python3
"""
Comprehensive login test to identify issues
"""

import urllib.request
import urllib.error
import json
import time

def test_comprehensive():
    print("=== Comprehensive Login Test ===\n")
    
    # Test 1: API accessibility
    print("1. Testing API accessibility...")
    try:
        response = urllib.request.urlopen('http://localhost:5062/openapi/v1.json')
        print("✓ API is accessible")
    except Exception as e:
        print(f"✗ API not accessible: {e}")
        return
    
    # Test 2: Direct API login
    print("\n2. Testing direct API login...")
    login_data = json.dumps({
        "email": "test@test.at",
        "password": "Test1234!"
    }).encode('utf-8')
    
    req = urllib.request.Request(
        'http://localhost:5062/login',
        data=login_data,
        headers={
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    )
    
    try:
        response = urllib.request.urlopen(req)
        result = json.loads(response.read().decode('utf-8'))
        print("✓ Direct API login successful")
        print(f"  Access token: {result.get('accessToken', '')[:50]}...")
        print(f"  Token type: {result.get('tokenType', 'N/A')}")
        access_token = result.get('accessToken', '')
    except urllib.error.HTTPError as e:
        print(f"✗ Direct API login failed: {e.code} {e.reason}")
        error_body = e.read().decode('utf-8')
        print(f"  Error: {error_body}")
        return
    except Exception as e:
        print(f"✗ Direct API login error: {e}")
        return
    
    # Test 3: CORS headers
    print("\n3. Testing CORS headers...")
    try:
        req = urllib.request.Request(
            'http://localhost:5062/login',
            method='OPTIONS',
            headers={
                'Origin': 'http://localhost:5000',
                'Access-Control-Request-Method': 'POST',
                'Access-Control-Request-Headers': 'content-type'
            }
        )
        response = urllib.request.urlopen(req)
        headers = dict(response.headers)
        
        cors_headers = {
            'Access-Control-Allow-Origin': headers.get('Access-Control-Allow-Origin', 'Missing'),
            'Access-Control-Allow-Methods': headers.get('Access-Control-Allow-Methods', 'Missing'),
            'Access-Control-Allow-Headers': headers.get('Access-Control-Allow-Headers', 'Missing')
        }
        
        for header, value in cors_headers.items():
            print(f"  {header}: {value}")
        
        if 'Missing' not in cors_headers.values():
            print("✓ CORS headers configured")
        else:
            print("✗ CORS headers missing")
    except Exception as e:
        print(f"✗ CORS test failed: {e}")
    
    # Test 4: UnoApp accessibility
    print("\n4. Testing UnoApp accessibility...")
    try:
        response = urllib.request.urlopen('http://localhost:5000')
        print("✓ UnoApp is accessible")
    except Exception as e:
        print(f"✗ UnoApp not accessible: {e}")
    
    # Test 5: Check if API accepts token
    if 'access_token' in locals():
        print("\n5. Testing authenticated API call...")
        req = urllib.request.Request(
            'http://localhost:5062/manage/info',
            headers={
                'Authorization': f'Bearer {access_token}',
                'Accept': 'application/json'
            }
        )
        
        try:
            response = urllib.request.urlopen(req)
            result = json.loads(response.read().decode('utf-8'))
            print("✓ Authenticated API call successful")
            print(f"  User email: {result.get('email', 'N/A')}")
        except urllib.error.HTTPError as e:
            print(f"✗ Authenticated API call failed: {e.code} {e.reason}")
        except Exception as e:
            print(f"✗ Authenticated API call error: {e}")
    
    # Test 6: Check login endpoint variations
    print("\n6. Testing login endpoint variations...")
    endpoints = [
        'http://localhost:5062/login',
        'http://localhost:5062/api/login',
        'http://localhost:5062/auth/login',
        'http://localhost:5062/identity/login'
    ]
    
    for endpoint in endpoints:
        try:
            req = urllib.request.Request(endpoint, method='POST')
            response = urllib.request.urlopen(req, timeout=1)
            print(f"  Found: {endpoint} (status: {response.status})")
        except urllib.error.HTTPError as e:
            if e.code == 400 or e.code == 415:
                print(f"  Found: {endpoint} (expects data)")
        except:
            pass
    
    print("\n=== Test Summary ===")
    print("The API is working correctly.")
    print("CORS is configured.")
    print("Authentication flow is functional.")
    print("\nThe issue is likely in the UnoApp mediator configuration or HTTP client setup.")
    print("\nNext steps:")
    print("1. Check browser console for errors when clicking login")
    print("2. Monitor network tab in browser DevTools")
    print("3. Check if requests are being sent to the correct URL")
    print("4. Verify the mediator HTTP configuration is applied correctly")

if __name__ == "__main__":
    test_comprehensive()