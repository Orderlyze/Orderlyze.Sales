---
description: 'Switch back to main branch and pull latest changes'
tools: ['Bash']
---
# Back to Main

Switches back to the main/master branch and pulls the latest changes from origin.

## Process

1. Checks out the main/master branch
2. Pulls latest changes from origin
3. Shows current branch and latest commit

## Usage

Simply run `/back-to-main` to return to your main branch with the latest updates.

The command will:
- Detect whether your main branch is called 'main' or 'master'
- Switch to that branch
- Pull the latest changes
- Display the current status

This is useful after working on feature branches and PRs to quickly get back to the updated main branch.