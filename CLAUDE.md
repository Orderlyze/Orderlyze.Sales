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

Use workflow automation for consistent GitHub workflow:
@.claude/templates/workflow-automation-scripts/implement-feature.py

### Quick Commands
- Build: `dotnet build`
- Run API: `dotnet run --project src/WebApi/WebApi.csproj`
- Run App: `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj`
- API Docs: http://localhost:5062/scalar/v1

### Critical Reminders
- **Always** add `[SingletonHandler]` attribute to Mediator handlers
- Follow the WebApi.json code generation workflow for new endpoints
- Check existing patterns in neighboring files before creating new code

## Standard Feature Implementation Workflow

**WICHTIG**: Bei Anfragen die mit "WORKFLOW:", "Implementiere:" oder "Feature:" beginnen, 
MUSS ich IMMER diese Schritte ausführen:

1. **GitHub Issue erstellen** (`gh issue create`)
2. **Feature Branch** (`git checkout -b feature/issue-XXX-name`)
3. **Implementierung** durchführen
4. **Build** (`dotnet build`) - bei Fehlern sofort fixen
5. **Commit & Push** mit Issue-Referenz
6. **Pull Request** (`gh pr create`) mit "Closes #XXX"

**NIEMALS** Code ohne Issue/PR!
**IMMER** Build grün vor PR!