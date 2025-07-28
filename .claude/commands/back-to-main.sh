#!/bin/bash

# Back to Main Command
# Switches to master branch and pulls latest changes

echo "🔄 Switching back to master branch..."

# Switch to master branch
git checkout master

if [ $? -eq 0 ]; then
    echo "✅ Successfully switched to master branch"
    
    # Pull latest changes
    echo "📥 Pulling latest changes..."
    git pull
    
    if [ $? -eq 0 ]; then
        echo "✅ Successfully pulled latest changes"
        
        # Show current status
        echo ""
        echo "📍 Current branch:"
        git branch --show-current
        
        echo ""
        echo "📝 Latest commit:"
        git log -1 --oneline
    else
        echo "❌ Failed to pull changes"
        exit 1
    fi
else
    echo "❌ Failed to switch to master branch"
    exit 1
fi