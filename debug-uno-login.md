# UnoApp Login Debugging Guide

## Summary of Issues Found

1. **API Configuration**: Was pointing to wrong URL (https://localhost:5001 instead of http://localhost:5062) - FIXED ✓
2. **CORS**: Was not configured in WebAPI - FIXED ✓ 
3. **Missing User**: Test user didn't exist in database - FIXED ✓
4. **Enhanced Logging**: Added detailed logging to authentication service - DONE ✓

## Current Status

The API is working correctly:
- Login endpoint responds with tokens
- CORS is properly configured
- Test user exists and can authenticate

## Browser Console Debugging Steps

1. Open http://localhost:5000 in your browser
2. Open Developer Tools (F12)
3. Go to the Console tab
4. Paste this script:

```javascript
// Enable verbose logging
localStorage.setItem('debug', '*');

// Hook into fetch to monitor API calls
const originalFetch = window.fetch;
window.fetch = function(...args) {
    console.log('🔵 FETCH REQUEST:', args[0], args[1]);
    return originalFetch.apply(this, args)
        .then(response => {
            console.log('🟢 FETCH RESPONSE:', response.status, response.statusText);
            return response;
        })
        .catch(error => {
            console.error('🔴 FETCH ERROR:', error);
            throw error;
        });
};

// Monitor XMLHttpRequest
const originalOpen = XMLHttpRequest.prototype.open;
XMLHttpRequest.prototype.open = function(method, url, ...args) {
    console.log('🔵 XHR REQUEST:', method, url);
    this.addEventListener('load', function() {
        console.log('🟢 XHR RESPONSE:', this.status, this.statusText);
    });
    this.addEventListener('error', function() {
        console.error('🔴 XHR ERROR');
    });
    return originalOpen.apply(this, [method, url, ...args]);
};

console.log('✅ API monitoring enabled. Now try to login.');
```

5. Try to login with:
   - Email: test@test.at
   - Password: Test1234!

6. Check the console for:
   - Any red error messages
   - API request details
   - Response status codes

## What to Look For

1. **No API calls**: If you don't see any blue FETCH/XHR messages, the issue is in the UI binding
2. **Wrong URL**: Check if requests go to the correct URL (http://localhost:5062/login)
3. **Request payload**: Verify the email and password are being sent correctly
4. **Response errors**: Look for 4xx or 5xx status codes

## Additional Debugging

If the login button doesn't trigger any API calls, check:

1. **Network Tab**: 
   - Clear the network log
   - Click login
   - Look for any failed requests

2. **Console Errors**:
   - Look for JavaScript errors before clicking login
   - Check for binding errors

3. **Application Logs**:
   - The enhanced logging should show messages in the console
   - Look for "LoginAsync called" and "Sending login request to API"

## Quick Test Commands

```bash
# Test API directly
curl -X POST http://localhost:5062/login \
  -H "Content-Type: application/json" \
  --data-binary @test-login.json

# Monitor UnoApp logs (in a new terminal)
dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj | grep -i "login\|auth"
```

## Next Steps

Based on what you find:
- If no API calls: Issue is in UI/ViewModel binding
- If wrong URL: Configuration issue
- If API returns error: Check request format
- If all looks good but still fails: Check response handling