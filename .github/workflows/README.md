# GitHub Actions Documentation

## Claude Code Review Action

This workflow automatically reviews pull requests using Claude AI.

### Setup Instructions

1. **Add the Claude API Key as a GitHub Secret:**
   - Go to your repository settings: https://github.com/Orderlyze/Orderlyze.Sales/settings/secrets/actions
   - Click "New repository secret"
   - Name: `CLAUDE_API_KEY`
   - Value: Your Claude API key (the one currently in use)
   - Click "Add secret"

2. **How it works:**
   - Triggers on pull request creation or updates
   - Extracts the PR diff and changed files
   - Sends to Claude for review (using Claude 3 Haiku for cost efficiency)
   - Posts the review as a comment on the PR

3. **Limitations:**
   - Reviews first 500 lines of diff to stay within token limits
   - Uses Claude 3 Haiku model for faster, more cost-effective reviews
   - Requires `CLAUDE_API_KEY` secret to be set

### Customization

To change the review focus, edit the prompt in `.github/workflows/claude-code-review.yml`.