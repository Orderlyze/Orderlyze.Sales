# GitHub Actions Documentation

## Claude Code Review Action

This workflow automatically reviews pull requests using Claude Code CLI.

### Setup Instructions

1. **Add the Claude API Key as a GitHub Secret:**
   - Go to your repository settings: https://github.com/Orderlyze/Orderlyze.Sales/settings/secrets/actions
   - Click "New repository secret"
   - Name: `CLAUDE_API_KEY`
   - Value: Your Claude API key (uses your subscription)
   - Click "Add secret"

2. **How it works:**
   - Triggers on pull request creation or updates
   - Installs Claude Code CLI in the GitHub runner
   - Extracts the PR diff and changed files
   - Uses Claude Code CLI to review the changes (leverages your subscription)
   - Posts the review as a comment on the PR

3. **Benefits:**
   - Uses Claude Code CLI instead of direct API calls
   - Leverages your Claude subscription pricing
   - More consistent with local development workflow
   - Uses Claude 3 Haiku model for fast reviews

4. **Limitations:**
   - Reviews first 500 lines of diff to stay within reasonable limits
   - Requires `CLAUDE_API_KEY` secret to be set

### Customization

To change the review focus, edit the prompt in the "Review with Claude Code" step in `.github/workflows/claude-code-review.yml`.