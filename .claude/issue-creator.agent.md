---
mode: 'agent'
description: 'Create GitHub issues based on feature requests with proper labeling and structure'
tools: ['githubRepo', 'runCommands']
---
# Issue Creator Agent

Create a GitHub issue for the feature request: `${input:FeatureDescription}`

## Issue Creation Process

1. **Parse feature details**:
   - Extract main feature title
   - Identify key requirements
   - Determine appropriate labels
   - Set priority level

2. **Structure issue content**:
   - **Title**: Clear, concise feature description
   - **Description**: 
     - User story format: "As a [user], I want [feature] so that [benefit]"
     - Acceptance criteria
     - Technical requirements
     - Dependencies (if any)
   - **Labels**: Select appropriate labels (enhancement, feature, priority)

3. **Create issue using gh CLI**:
   ```bash
   gh issue create --title "[title]" --body "[body]" --label "[labels]"
   ```

## Issue Template

```markdown
## Feature Request: [Feature Name]

### User Story
As a [user type], I want [feature description] so that [benefit/value].

### Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2
- [ ] Criterion 3

### Technical Requirements
- Requirement 1
- Requirement 2

### Dependencies
- List any dependencies

### Additional Context
[Any additional information]
```

## Output Format

Return the created issue details:
```json
{
  "issueNumber": 123,
  "issueUrl": "https://github.com/...",
  "title": "Issue title",
  "labels": ["enhancement", "feature"],
  "body": "Full issue body content"
}
```