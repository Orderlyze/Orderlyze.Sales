---
name: 'pr-creator'
description: 'Create pull requests for implemented features with proper branch management and PR structure'
tools: ['githubRepo', 'runCommands', 'codebase', 'editFiles', 'search']
---
# PR Creator Agent

Create a pull request for the feature: `${input:FeatureDescription}`
Related to issue: `${input:IssueNumber}`

## PR Creation Process

1. **Branch Management**:
   - Create feature branch: `feature/issue-${IssueNumber}-[feature-slug]`
   - Ensure branch is up to date with master
   - Commit all changes with proper commit messages

2. **Implementation**:
   - Follow CLAUDE.md guidelines
   - Implement the feature according to requirements
   - Ensure code follows project conventions
   - Run `dotnet build` to verify compilation

3. **PR Structure**:
   - **Title**: `feat: [Feature Description] (#${IssueNumber})`
   - **Description**:
     - Link to issue
     - Summary of changes
     - Testing performed
     - Screenshots (if UI changes)
   - **Labels**: Appropriate labels based on change type

4. **Create PR**:
   ```bash
   gh pr create --title "[title]" --body "[body]" --base master --head [branch]
   ```

## PR Template

```markdown
## Summary
Brief description of what this PR does.

Closes #${IssueNumber}

## Changes
- Change 1
- Change 2
- Change 3

## Testing
- [ ] Code compiles without warnings (`dotnet build`)
- [ ] Manual testing completed
- [ ] UI changes verified (if applicable)

## Screenshots
[If applicable]

## Additional Notes
[Any additional context]
```

## Output Format

Return PR details:
```json
{
  "prNumber": 456,
  "prUrl": "https://github.com/...",
  "branch": "feature/issue-123-feature-name",
  "title": "PR title",
  "linkedIssue": 123,
  "status": "ready" | "draft"
}
```