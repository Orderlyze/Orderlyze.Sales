// Browser console script to test login
// To use: Open http://localhost:5000 in browser, open DevTools console, paste this script

async function testLogin() {
    console.log('Testing login functionality...');
    
    // Check if we're on the right page
    if (!window.location.href.includes('localhost:5000')) {
        console.error('Please navigate to http://localhost:5000 first');
        return;
    }
    
    // Wait for page to load
    await new Promise(resolve => setTimeout(resolve, 2000));
    
    // Try to find login form elements
    const emailInput = document.querySelector('input[type="text"][placeholder*="Email" i], input[type="email"]');
    const passwordInput = document.querySelector('input[type="password"]');
    const loginButton = document.querySelector('button:has-text("Login"), button:contains("Login")') || 
                       Array.from(document.querySelectorAll('button')).find(btn => btn.textContent.includes('Login'));
    
    if (!emailInput || !passwordInput || !loginButton) {
        console.error('Could not find login form elements');
        console.log('Email input:', emailInput);
        console.log('Password input:', passwordInput);
        console.log('Login button:', loginButton);
        return;
    }
    
    // Fill in credentials
    emailInput.value = 'test@test.at';
    emailInput.dispatchEvent(new Event('input', { bubbles: true }));
    emailInput.dispatchEvent(new Event('change', { bubbles: true }));
    
    passwordInput.value = 'Test1234!';
    passwordInput.dispatchEvent(new Event('input', { bubbles: true }));
    passwordInput.dispatchEvent(new Event('change', { bubbles: true }));
    
    console.log('Credentials filled, clicking login button...');
    
    // Click login
    loginButton.click();
    
    // Monitor network requests
    console.log('Monitoring network activity...');
    
    // Check console for errors
    const originalError = console.error;
    console.error = function(...args) {
        console.log('Console error detected:', ...args);
        originalError.apply(console, args);
    };
}

// Run the test
testLogin();