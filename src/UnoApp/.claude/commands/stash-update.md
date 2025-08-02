# Stash and Update Command

This slash command helps you quickly stash your current changes, checkout the main branch, and pull the latest updates.

## Usage
```
/stash-update
```

## What it does:
1. Stashes all current changes (if any exist)
2. Checks out the master branch
3. Pulls the latest changes from remote

## Workflow:

### 1. Check for uncommitted changes
```bash
git status --porcelain
```

### 2. If changes exist, stash them
```bash
git stash push -m "Auto-stash before updating master"
```

### 3. Checkout master branch
```bash
git checkout master
```

### 4. Pull latest changes
```bash
git pull origin master
```

### 5. Show stash list (if any)
```bash
git stash list
```

## Options:
- To retrieve your stashed changes later, use: `git stash pop`
- To see what's in the stash: `git stash show -p`

## Notes:
- This command is useful when you need to quickly switch to an updated master branch
- Your work in progress is safely stored in the stash
- Remember to pop your stash when returning to your feature branch