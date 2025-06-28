# Codex Instructions

## General Guidelines
- Use C# 12 (.NET 9) features.
- Indent C# code with four spaces.
- End every file with a newline.
- Prefer file-scoped namespaces when practical.

## Shiny Mediator Usage
- The API and UnoApp are built around the **Shiny.Mediator** pattern.
- When adding or updating endpoints or features, create request and handler classes and decorate them with the appropriate `MediatorHttp*` attributes.
- API requests belong under `src/WebApi/Mediator/Requests` and handlers under `src/WebApi/Mediator/Handlers`.
- UnoApp features should access the API using generated mediator clients rather than raw HTTP calls.

## Testing
- Do not do any testing

## Pull Requests
- Report the result of the build commands.
