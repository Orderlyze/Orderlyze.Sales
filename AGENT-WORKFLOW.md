# Automated Issue & PR Management Workflow

This document describes the automated workflow for handling feature requests, creating GitHub issues, and preparing pull requests.

## Overview

The system consists of specialized agents that work together to:
- Analyze feature requests like a Product Owner
- Check for existing GitHub issues
- Create new issues when needed
- Prepare and create pull requests for implementation

## Agent Roles

### 1. Product Owner Agent (`.claude/product-owner.agent.md`)
- **Role**: Main coordinator and decision maker
- **Responsibilities**:
  - Analyzes incoming feature requests
  - Coordinates other agents
  - Makes decisions about issue/PR structure
  - Provides comprehensive action plans

### 2. Feature Analyzer Agent (`.claude/feature-analyzer.agent.md`)
- **Role**: Technical analysis specialist
- **Responsibilities**:
  - Breaks down feature requests into components
  - Assesses technical complexity
  - Identifies dependencies
  - Recommends implementation approach

### 3. Issue Analyzer Agent (`.claude/issue-analyzer.agent.md`)
- **Role**: GitHub issue specialist
- **Responsibilities**:
  - Searches for existing issues
  - Analyzes similarity to requested features
  - Recommends create new vs update existing

### 4. Issue Creator Agent (`.claude/issue-creator.agent.md`)
- **Role**: GitHub issue creation
- **Responsibilities**:
  - Creates well-structured GitHub issues
  - Applies appropriate labels
  - Links related issues
  - Follows issue templates

### 5. PR Creator Agent (`.claude/pr-creator.agent.md`)
- **Role**: Pull request specialist
- **Responsibilities**:
  - Creates feature branches
  - Implements features following project guidelines
  - Creates pull requests with proper structure
  - Links PRs to issues

## Usage

### Quick Start

Use the automated workflow prompt:
```
/automated-workflow "Your feature request description here"
```

### Manual Agent Usage

You can also use individual agents:
```
/product-owner "Analyze this feature request: ..."
/feature-analyzer "Break down this feature: ..."
/issue-analyzer "Check if issues exist for: ..."
/issue-creator "Create issue for: ..."
/pr-creator "Create PR for issue #123"
```

## Workflow Process

1. **Request Analysis**
   - Product Owner agent receives the request
   - Feature Analyzer breaks it down into components
   - Complexity and dependencies are assessed

2. **Issue Management**
   - Issue Analyzer checks for existing issues
   - Issue Creator creates new issues as needed
   - Issues are properly labeled and linked

3. **Development Preparation**
   - PR Creator prepares branches
   - Implementation follows project guidelines
   - PRs are created and linked to issues

## Example Workflow

**User Request**: "Add a customer dashboard with sales metrics"

**System Response**:
1. Analyzes request → identifies 3 sub-features
2. Checks existing issues → finds 1 related, needs 2 new
3. Creates 2 new GitHub issues
4. Prepares 3 feature branches
5. Returns comprehensive report with:
   - Issue numbers: #124, #125
   - PR branches ready for implementation
   - Next steps for development

## Benefits

- **Consistency**: All issues and PRs follow the same structure
- **Efficiency**: Automated checks prevent duplicate work
- **Traceability**: Clear links from request → issue → PR
- **Quality**: Follows project guidelines automatically
- **Transparency**: Acts like a real Product Owner

## Configuration

Agents are configured in the `.claude/` directory:
- Each agent has its own `.agent.md` file
- Agents can be customized for project needs
- The automated workflow prompt coordinates all agents

## Best Practices

1. **Be Specific**: Provide detailed feature descriptions
2. **Check Output**: Review created issues before implementation
3. **Link Everything**: Ensure issues and PRs are properly linked
4. **Follow Up**: Use the suggested next steps from the workflow
5. **Iterate**: Complex features may need multiple rounds

## Troubleshooting

If the workflow encounters issues:
- Check GitHub CLI authentication: `gh auth status`
- Ensure you're on the correct branch
- Verify repository permissions
- Review agent output for specific errors