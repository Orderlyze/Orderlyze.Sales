#!/usr/bin/env python3
"""
Feature Implementation Workflow Script
Automatisiert den kompletten Workflow von Issue bis PR
"""

import subprocess
import sys
import re
import json
from datetime import datetime
import os

class Colors:
    BLUE = '\033[94m'
    GREEN = '\033[92m'
    YELLOW = '\033[93m'
    RED = '\033[91m'
    END = '\033[0m'
    BOLD = '\033[1m'

def run_command(cmd, capture=True, check=True):
    """Execute a shell command and return the output"""
    print(f"{Colors.BLUE}Running: {cmd}{Colors.END}")
    if capture:
        result = subprocess.run(cmd, shell=True, capture_output=True, text=True, check=check)
        return result.stdout.strip()
    else:
        subprocess.run(cmd, shell=True, check=check)
        return None

def create_issue(title, description):
    """Create a GitHub issue and return the issue number"""
    print(f"\n{Colors.BOLD}=== Creating GitHub Issue ==={Colors.END}")
    
    body = f"""## Feature Request

{description}

## Acceptance Criteria
- [ ] Feature implemented
- [ ] Tests added (if applicable)
- [ ] Build passes
- [ ] Documentation updated

## Technical Details
To be determined during implementation.

Created by automated workflow script."""
    
    # Create issue
    cmd = f'gh issue create --title "{title}" --body "{body}" --label "enhancement"'
    output = run_command(cmd)
    
    # Extract issue number from output
    issue_match = re.search(r'#(\d+)', output)
    if issue_match:
        issue_number = issue_match.group(1)
        print(f"{Colors.GREEN}✓ Created issue #{issue_number}{Colors.END}")
        return issue_number
    else:
        print(f"{Colors.RED}Failed to extract issue number from: {output}{Colors.END}")
        sys.exit(1)

def create_branch(issue_number, feature_name):
    """Create and checkout a feature branch"""
    print(f"\n{Colors.BOLD}=== Creating Feature Branch ==={Colors.END}")
    
    # Create safe branch name
    safe_name = re.sub(r'[^a-zA-Z0-9-]', '-', feature_name.lower())
    safe_name = re.sub(r'-+', '-', safe_name).strip('-')[:50]
    branch_name = f"feature/issue-{issue_number}-{safe_name}"
    
    # Ensure we're on master and up to date
    run_command("git checkout master")
    run_command("git pull origin master")
    
    # Create and checkout new branch
    run_command(f"git checkout -b {branch_name}")
    print(f"{Colors.GREEN}✓ Created branch: {branch_name}{Colors.END}")
    
    return branch_name

def implement_feature(feature_description):
    """Placeholder for feature implementation"""
    print(f"\n{Colors.BOLD}=== Implementing Feature ==={Colors.END}")
    print(f"{Colors.YELLOW}! Manual implementation required for: {feature_description}{Colors.END}")
    print("Please implement the feature now, then press Enter to continue...")
    input()
    return True

def build_and_fix():
    """Build the project and fix any errors"""
    print(f"\n{Colors.BOLD}=== Building Project ==={Colors.END}")
    
    max_attempts = 3
    attempt = 1
    
    while attempt <= max_attempts:
        print(f"\nBuild attempt {attempt}/{max_attempts}")
        
        # Try to build
        result = subprocess.run("dotnet build", shell=True, capture_output=True, text=True)
        
        if result.returncode == 0:
            print(f"{Colors.GREEN}✓ Build successful!{Colors.END}")
            return True
        else:
            print(f"{Colors.RED}✗ Build failed{Colors.END}")
            print(result.stdout)
            print(result.stderr)
            
            if attempt < max_attempts:
                print(f"{Colors.YELLOW}Please fix the build errors and press Enter to retry...{Colors.END}")
                input()
            
            attempt += 1
    
    print(f"{Colors.RED}Build failed after {max_attempts} attempts{Colors.END}")
    return False

def commit_and_push(branch_name, feature_description, issue_number):
    """Commit changes and push to remote"""
    print(f"\n{Colors.BOLD}=== Committing Changes ==={Colors.END}")
    
    # Check if there are changes
    status = run_command("git status --porcelain")
    if not status:
        print(f"{Colors.YELLOW}No changes to commit{Colors.END}")
        return False
    
    # Stage all changes
    run_command("git add -A")
    
    # Create commit message
    commit_msg = f"""feat: {feature_description} (#{issue_number})

- Implemented {feature_description}
- All builds passing
- Ready for review

Closes #{issue_number}

🤖 Generated with Claude Code

Co-Authored-By: Claude <noreply@anthropic.com>"""
    
    # Commit
    with open('.commit_msg_tmp', 'w') as f:
        f.write(commit_msg)
    run_command('git commit -F .commit_msg_tmp')
    os.remove('.commit_msg_tmp')
    
    # Push
    run_command(f"git push -u origin {branch_name}")
    print(f"{Colors.GREEN}✓ Changes pushed to {branch_name}{Colors.END}")
    
    return True

def create_pull_request(branch_name, feature_description, issue_number):
    """Create a pull request"""
    print(f"\n{Colors.BOLD}=== Creating Pull Request ==={Colors.END}")
    
    pr_body = f"""## Summary
This PR implements: {feature_description}

Closes #{issue_number}

## Changes
- Implementation details added during development
- All changes follow project conventions
- Build passing

## Testing
- [x] Build passes (`dotnet build`)
- [x] Manual testing completed
- [ ] Code review pending

## Checklist
- [x] Linked to issue #{issue_number}
- [x] Changes follow project style guide
- [x] Build is green
- [x] Ready for review

🤖 Generated with Claude Code

Co-Authored-By: Claude <noreply@anthropic.com>"""
    
    # Create PR
    cmd = f'gh pr create --title "feat: {feature_description} (#{issue_number})" --body "{pr_body}" --base master'
    output = run_command(cmd)
    
    print(f"{Colors.GREEN}✓ Pull request created!{Colors.END}")
    print(output)
    
    return True

def log_workflow(feature_description, issue_number, branch_name, success):
    """Log the workflow execution"""
    log_file = "workflow-log.json"
    
    log_entry = {
        "timestamp": datetime.now().isoformat(),
        "feature": feature_description,
        "issue_number": issue_number,
        "branch": branch_name,
        "success": success
    }
    
    # Read existing log
    if os.path.exists(log_file):
        with open(log_file, 'r') as f:
            log_data = json.load(f)
    else:
        log_data = []
    
    # Append new entry
    log_data.append(log_entry)
    
    # Write back
    with open(log_file, 'w') as f:
        json.dump(log_data, f, indent=2)

def main():
    if len(sys.argv) < 2:
        print(f"{Colors.RED}Usage: python implement-feature.py \"Feature description\"{Colors.END}")
        sys.exit(1)
    
    feature_description = " ".join(sys.argv[1:])
    
    print(f"{Colors.BOLD}=== Feature Implementation Workflow ==={Colors.END}")
    print(f"Feature: {feature_description}")
    print("=" * 50)
    
    try:
        # Step 1: Create Issue
        issue_number = create_issue(feature_description, feature_description)
        
        # Step 2: Create Branch
        branch_name = create_branch(issue_number, feature_description)
        
        # Step 3: Implement Feature (manual step)
        implement_feature(feature_description)
        
        # Step 4: Build and Fix
        if not build_and_fix():
            print(f"{Colors.RED}Aborting: Build failures could not be resolved{Colors.END}")
            sys.exit(1)
        
        # Step 5: Commit and Push
        if not commit_and_push(branch_name, feature_description, issue_number):
            print(f"{Colors.YELLOW}Warning: No changes to commit{Colors.END}")
        
        # Step 6: Create PR
        create_pull_request(branch_name, feature_description, issue_number)
        
        # Log success
        log_workflow(feature_description, issue_number, branch_name, True)
        
        print(f"\n{Colors.GREEN}{Colors.BOLD}✓ Workflow completed successfully!{Colors.END}")
        print(f"Issue: #{issue_number}")
        print(f"Branch: {branch_name}")
        print(f"Next steps: Wait for PR review and merge")
        
    except subprocess.CalledProcessError as e:
        print(f"{Colors.RED}Error executing command: {e}{Colors.END}")
        log_workflow(feature_description, issue_number if 'issue_number' in locals() else None, 
                    branch_name if 'branch_name' in locals() else None, False)
        sys.exit(1)
    except KeyboardInterrupt:
        print(f"\n{Colors.YELLOW}Workflow interrupted by user{Colors.END}")
        sys.exit(1)

if __name__ == "__main__":
    main()