#!/usr/bin/env python3
"""
Simple test script to check UnoApp login functionality
"""

import time
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.options import Options

def test_login():
    # Setup Chrome options for headless mode
    chrome_options = Options()
    chrome_options.add_argument("--headless")
    chrome_options.add_argument("--no-sandbox")
    chrome_options.add_argument("--disable-dev-shm-usage")
    
    # Create driver
    driver = webdriver.Chrome(options=chrome_options)
    
    try:
        # Navigate to the app
        print("Navigating to UnoApp...")
        driver.get("http://localhost:5000")
        
        # Wait for the page to load
        print("Waiting for page to load...")
        time.sleep(5)
        
        # Get page title and current URL
        print(f"Page title: {driver.title}")
        print(f"Current URL: {driver.current_url}")
        
        # Get page source (first 500 chars)
        print(f"Page source (first 500 chars): {driver.page_source[:500]}...")
        
        # Try to find login elements
        try:
            email_field = driver.find_element(By.NAME, "EmailTextBox")
            print("Found email field!")
        except:
            print("Email field not found")
            
        try:
            password_field = driver.find_element(By.NAME, "PasswordBox")
            print("Found password field!")
        except:
            print("Password field not found")
            
        # Check for any error messages
        try:
            errors = driver.find_elements(By.CLASS_NAME, "error")
            if errors:
                print(f"Found {len(errors)} error elements")
                for error in errors:
                    print(f"Error text: {error.text}")
        except:
            pass
            
        # Get console logs
        print("\nBrowser console logs:")
        for log in driver.get_log('browser'):
            print(log)
            
    finally:
        driver.quit()

if __name__ == "__main__":
    test_login()