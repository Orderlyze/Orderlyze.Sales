# Claude Code Hooks Setup

## Current Configuration

Hooks are configured in `/home/daniel/.config/claude/settings.json` to automatically log all changes made by Claude.

### Hook Script
Located at: `/home/daniel/Orderlyze.Sales/.claude-hooks/log-change.py`

This script logs:
- All file edits/writes
- Commands executed
- Patterns of changes (for learning)
- Git branch context

### Log Output
Changes are logged to: `/home/daniel/Orderlyze.Sales/claude-changes.json`

## Important Notes

1. **Hooks are loaded at Claude Code startup** - Changes to hook configuration require restarting Claude Code
2. The current session may not have the hooks active yet
3. In future sessions, all changes will be automatically logged

## Testing Hooks

To manually test if hooks are working:
```bash
# Create a test file
echo "test" > test.txt

# Check if logged
cat /home/daniel/Orderlyze.Sales/claude-changes.json
```

## Hook Configuration

The hooks capture:
- `Write` - New file creation
- `Edit` - File modifications  
- `MultiEdit` - Multiple edits to same file
- `Bash` - Shell commands executed

Each log entry includes:
- Timestamp
- Change type
- File path or command
- Current working directory
- Git branch

This ensures all changes are tracked for learning and auditing purposes.