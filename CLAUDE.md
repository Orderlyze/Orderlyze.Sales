# Orderlyze.Sales - Claude Configuration

Quick access to project context and commands for efficient AI-assisted development.

## Project Context
@.claude/project-overview.md
@.claude/architecture.md
@.claude/project-structure.md

## Development
@.claude/commands.md
@.claude/code-generation.md

## Specialized Modes
@.claude/development-modes/expert-dotnet.md
@.claude/development-modes/create-adr.md

## Standard Workflows

### Feature Implementation
When requests begin with "WORKFLOW:", "Implementiere:", or "Feature:", follow these steps:
1. **GitHub Issue** (`gh issue create`)
2. **Feature Branch** (`git checkout -b feature/issue-XXX-name`)
3. **Implementation**
4. **Build** (`dotnet build`) - fix errors before proceeding
5. **Commit & Push** with issue reference
6. **Pull Request** (`gh pr create`) with "Closes #XXX"

### Quick Commands
- Build: `dotnet build`
- Run API: `dotnet run --project src/WebApi/WebApi.csproj`
- Run App: `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj`
- API Docs: http://localhost:5062/scalar/v1

### Critical Reminders
- **Always** add `[SingletonHandler]` attribute to Mediator handlers
- Follow the WebApi.json code generation workflow for new endpoints
- Check existing patterns in neighboring files before creating new code