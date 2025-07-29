#!/usr/bin/env python3
"""
Automated Feature Implementation Script
Führt den kompletten Workflow automatisch aus, inklusive Code-Generierung
"""

import subprocess
import sys
import re
import json
import os
import tempfile
from datetime import datetime

class AutoImplement:
    def __init__(self):
        self.colors = {
            'blue': '\033[94m',
            'green': '\033[92m',
            'yellow': '\033[93m',
            'red': '\033[91m',
            'end': '\033[0m',
            'bold': '\033[1m'
        }
        
    def log(self, message, color='blue'):
        """Print colored log message"""
        print(f"{self.colors[color]}{message}{self.colors['end']}")
        
    def run_cmd(self, cmd, capture=True):
        """Execute command and return output"""
        self.log(f"$ {cmd}", 'blue')
        if capture:
            result = subprocess.run(cmd, shell=True, capture_output=True, text=True)
            if result.returncode != 0:
                self.log(f"Error: {result.stderr}", 'red')
                raise subprocess.CalledProcessError(result.returncode, cmd)
            return result.stdout.strip()
        else:
            subprocess.check_call(cmd, shell=True)
            return None
    
    def create_issue(self, title, description):
        """Create GitHub issue"""
        self.log(f"\n=== Creating GitHub Issue ===", 'bold')
        
        body = f"""## Feature Request
{description}

## Implementation Plan
Automated implementation via script

## Acceptance Criteria
- [ ] Feature implemented
- [ ] Build passes
- [ ] Tests added (if applicable)
- [ ] PR created

Created by auto-implement script."""
        
        # Save body to temp file to avoid escaping issues
        with tempfile.NamedTemporaryFile(mode='w', suffix='.md', delete=False) as f:
            f.write(body)
            temp_file = f.name
        
        try:
            output = self.run_cmd(f'gh issue create --title "{title}" --body-file {temp_file} --label "enhancement"')
            os.unlink(temp_file)
            
            match = re.search(r'#(\d+)', output)
            if match:
                issue_num = match.group(1)
                self.log(f"✓ Created issue #{issue_num}", 'green')
                return issue_num
            else:
                raise Exception("Could not extract issue number")
        finally:
            if os.path.exists(temp_file):
                os.unlink(temp_file)
    
    def create_branch(self, issue_number, feature_name):
        """Create feature branch"""
        self.log(f"\n=== Creating Feature Branch ===", 'bold')
        
        # Clean branch name
        safe_name = re.sub(r'[^a-zA-Z0-9-]', '-', feature_name.lower())
        safe_name = re.sub(r'-+', '-', safe_name).strip('-')[:50]
        branch = f"feature/issue-{issue_number}-{safe_name}"
        
        self.run_cmd("git checkout master")
        self.run_cmd("git pull origin master")
        self.run_cmd(f"git checkout -b {branch}")
        
        self.log(f"✓ Created branch: {branch}", 'green')
        return branch
    
    def implement_claude_code(self, feature_description, issue_number):
        """Use Claude to implement the feature"""
        self.log(f"\n=== Implementing Feature with Claude ===", 'bold')
        
        # Create a Claude instruction file
        instruction = f"""Implement this feature: {feature_description}

Requirements:
1. Follow all project conventions in CLAUDE.md
2. Use existing styles from Resources/Styles/
3. Ensure all XAML uses style resources, no hardcoded values
4. Add appropriate error handling
5. Follow MVVM pattern

Related to issue #{issue_number}

IMPORTANT: Make all necessary code changes to implement this feature completely."""
        
        with tempfile.NamedTemporaryFile(mode='w', suffix='.txt', delete=False) as f:
            f.write(instruction)
            instruction_file = f.name
        
        self.log("Instruction file created. Please implement the feature based on the instructions.", 'yellow')
        self.log(f"Feature: {feature_description}", 'yellow')
        self.log("Press Enter when implementation is complete...", 'yellow')
        input()
        
        os.unlink(instruction_file)
        return True
    
    def build_until_success(self, max_attempts=5):
        """Build and auto-fix until successful"""
        self.log(f"\n=== Building Project ===", 'bold')
        
        for attempt in range(1, max_attempts + 1):
            self.log(f"\nBuild attempt {attempt}/{max_attempts}")
            
            result = subprocess.run("dotnet build", shell=True, capture_output=True, text=True)
            
            if result.returncode == 0:
                self.log("✓ Build successful!", 'green')
                return True
            else:
                self.log("✗ Build failed", 'red')
                if attempt < max_attempts:
                    self.log("Attempting to fix build errors...", 'yellow')
                    self.log("Please fix the errors and press Enter...", 'yellow')
                    input()
        
        return False
    
    def commit_and_push(self, branch, feature, issue_number):
        """Commit and push changes"""
        self.log(f"\n=== Committing Changes ===", 'bold')
        
        # Check for changes
        status = self.run_cmd("git status --porcelain")
        if not status:
            self.log("No changes to commit", 'yellow')
            return False
        
        self.run_cmd("git add -A")
        
        commit_msg = f"""feat: {feature} (#{issue_number})

- Implemented {feature}
- All builds passing
- Ready for review

Closes #{issue_number}

🤖 Generated with Claude Code

Co-Authored-By: Claude <noreply@anthropic.com>"""
        
        with tempfile.NamedTemporaryFile(mode='w', suffix='.txt', delete=False) as f:
            f.write(commit_msg)
            msg_file = f.name
        
        try:
            self.run_cmd(f"git commit -F {msg_file}")
            os.unlink(msg_file)
            self.run_cmd(f"git push -u origin {branch}")
            self.log(f"✓ Pushed to {branch}", 'green')
            return True
        finally:
            if os.path.exists(msg_file):
                os.unlink(msg_file)
    
    def create_pr(self, branch, feature, issue_number):
        """Create pull request"""
        self.log(f"\n=== Creating Pull Request ===", 'bold')
        
        pr_body = f"""## Summary
Implements: {feature}

Closes #{issue_number}

## Changes
- Implementation completed via automated workflow
- All builds passing
- Ready for review

## Testing
- [x] Build passes
- [x] Automated workflow completed
- [ ] Manual testing pending
- [ ] Code review pending

🤖 Generated with Claude Code"""
        
        with tempfile.NamedTemporaryFile(mode='w', suffix='.md', delete=False) as f:
            f.write(pr_body)
            body_file = f.name
        
        try:
            output = self.run_cmd(
                f'gh pr create --title "feat: {feature} (#{issue_number})" '
                f'--body-file {body_file} --base master'
            )
            os.unlink(body_file)
            self.log("✓ Pull request created!", 'green')
            print(output)
            return True
        finally:
            if os.path.exists(body_file):
                os.unlink(body_file)
    
    def run(self, feature_description):
        """Run the complete workflow"""
        self.log(f"{self.colors['bold']}=== Auto Implementation Workflow ==={self.colors['end']}")
        self.log(f"Feature: {feature_description}")
        print("=" * 50)
        
        try:
            # Create issue
            issue_number = self.create_issue(feature_description, feature_description)
            
            # Create branch
            branch = self.create_branch(issue_number, feature_description)
            
            # Implement with Claude
            self.implement_claude_code(feature_description, issue_number)
            
            # Build until success
            if not self.build_until_success():
                self.log("Failed to fix build errors", 'red')
                return False
            
            # Commit and push
            if self.commit_and_push(branch, feature_description, issue_number):
                # Create PR
                self.create_pr(branch, feature_description, issue_number)
            
            self.log(f"\n✓ Workflow completed successfully!", 'green')
            self.log(f"Issue: #{issue_number}")
            self.log(f"Branch: {branch}")
            
            return True
            
        except Exception as e:
            self.log(f"Error: {str(e)}", 'red')
            return False

def main():
    if len(sys.argv) < 2:
        print("Usage: python auto-implement.py \"Feature description\"")
        sys.exit(1)
    
    feature = " ".join(sys.argv[1:])
    
    implementer = AutoImplement()
    success = implementer.run(feature)
    
    sys.exit(0 if success else 1)

if __name__ == "__main__":
    main()