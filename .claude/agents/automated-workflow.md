---
name: 'automated-workflow'
description: 'Automated workflow to analyze feature requests, create issues, and prepare pull requests'
tools: ['*']
---
# Automated Feature Development Workflow

Process the feature request and coordinate the complete development workflow.

## Usage

Simply provide your feature request in natural language, and this workflow will:
1. Analyze the request as a Product Owner would
2. Check for existing GitHub issues
3. Create new issues if needed
4. Prepare pull requests for implementation

## Example Usage

```
/automated-workflow "Add a dashboard that shows sales metrics with daily, weekly, and monthly views"
```

## Workflow Steps

1. **Feature Analysis**
   - Use the feature-analyzer agent to break down the request
   - Identify technical requirements and complexity

2. **Issue Management**
   - Use issue-analyzer to check for existing issues
   - Use issue-creator to create new issues as needed

3. **Development Planning**
   - Determine implementation approach
   - Plan PR structure based on feature breakdown

4. **Implementation Coordination**
   - Use pr-creator agent for each feature component
   - Ensure proper linking between issues and PRs

## Process

The workflow will:
- Act as a Product Owner using the product-owner agent
- Provide a complete analysis and action plan
- Create all necessary GitHub issues
- Prepare the development branches and PRs
- Return a summary of all created artifacts

The user will receive a comprehensive report including:
- Feature analysis breakdown
- Created issue numbers and links
- Prepared PR branches
- Next steps for implementation