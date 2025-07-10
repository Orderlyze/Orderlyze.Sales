# Orderlyze.Sales - Claude Memory

This file provides context for Claude Code when working on the Orderlyze.Sales project.

## Quick Reference

### Most Used Commands
- Build: `dotnet build`
- Run API: `dotnet run --project src/WebApi/WebApi.csproj`
- Run App: `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj`
- API Docs: http://localhost:5062/scalar/v1

### Critical Reminders
- **Always** add `[SingletonHandler]` attribute to Mediator handlers
- Follow the WebApi.json code generation workflow for new endpoints
- Check existing patterns in neighboring files before creating new code

## Project Context

@.claude/project-overview.md
@.claude/commands.md
@.claude/architecture.md
@.claude/project-structure.md

## Special Workflows

@.claude/code-generation.md

## Development Modes & Tools

@.claude/expert-dotnet-software-engineer.chatmode.md
@.claude/create-architectural-decision-record.prompt.md

## Development Notes

### Environment Specifics
- .NET 9 is used throughout the project
- WebAssembly builds require special environment variable handling
- Git submodules are used (especially for mediator)

### Change Tracking
- All changes are logged to `/claude-changes.json`

### Testing Approach
- Always check README or search codebase for test commands
- Run lint and typecheck commands if found
- Verify builds before committing