---
mode: 'agent'
description: 'Analyze if GitHub issues already exist for a given feature request and provide detailed analysis'
tools: ['githubRepo', 'search', 'searchResults']
---
# Issue Analyzer Agent

Analyze if GitHub issues already exist for the feature request: `${input:FeatureDescription}`

## Analysis Requirements

1. **Search for existing issues**:
   - Use `gh issue list --search` with relevant keywords from the feature description
   - Check both open and closed issues
   - Look for similar feature requests, not just exact matches

2. **Analyze found issues**:
   - For each relevant issue found, document:
     - Issue number and title
     - Current status (open/closed)
     - Similarity score (high/medium/low)
     - Key differences from the requested feature

3. **Provide recommendation**:
   - If exact match exists: Return issue number
   - If similar exists: Suggest updating existing issue vs creating new
   - If none exist: Confirm new issue should be created

## Output Format

Return a structured JSON response:
```json
{
  "existingIssues": [
    {
      "number": 123,
      "title": "Issue title",
      "status": "open",
      "similarity": "high",
      "differences": ["list of key differences"]
    }
  ],
  "recommendation": "create_new" | "use_existing" | "update_existing",
  "recommendedIssueNumber": null | 123,
  "reasoning": "Detailed explanation of recommendation"
}
```