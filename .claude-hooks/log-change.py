#!/usr/bin/env python3
import json
import sys
import os
from datetime import datetime
from pathlib import Path

def log_change(change_type, file_path=None, command=None, old_content=None, new_content=None):
    log_file = Path("/home/daniel/Orderlyze.Sales/claude-changes.json")
    
    # Initialize log file if it doesn't exist
    if not log_file.exists():
        log_file.write_text(json.dumps({"changes": [], "patterns": {}, "common_fixes": []}, indent=2))
    
    # Load existing data
    with open(log_file, 'r') as f:
        data = json.load(f)
    
    # Create change entry
    change = {
        "timestamp": datetime.now().isoformat(),
        "type": change_type,
        "file_path": file_path,
        "command": command,
        "context": {
            "cwd": os.getcwd(),
            "branch": os.popen("git branch --show-current 2>/dev/null").read().strip()
        }
    }
    
    # Add to changes list
    data["changes"].append(change)
    
    # Analyze patterns (for learning)
    if file_path:
        ext = Path(file_path).suffix
        if ext not in data["patterns"]:
            data["patterns"][ext] = {"edits": 0, "creates": 0}
        
        if change_type in ["edit", "multiedit"]:
            data["patterns"][ext]["edits"] += 1
        elif change_type == "write":
            data["patterns"][ext]["creates"] += 1
    
    # Keep only last 1000 changes
    if len(data["changes"]) > 1000:
        data["changes"] = data["changes"][-1000:]
    
    # Save updated data
    with open(log_file, 'w') as f:
        json.dump(data, f, indent=2)

if __name__ == "__main__":
    args = sys.argv[1:]
    if len(args) >= 2:
        change_type = args[0]
        if change_type in ["edit", "write", "multiedit"]:
            log_change(change_type, file_path=args[1])
        elif change_type == "bash":
            log_change(change_type, command=args[1])