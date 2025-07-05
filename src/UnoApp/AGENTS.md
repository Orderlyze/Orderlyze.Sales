# UnoApp Guidelines

UnoApp communicates with the API through Shiny Mediator.

- Prefer generated mediator clients over raw `HttpClient` or Refit when calling the API.
- View models should send mediator requests and process the responses asynchronously.
- Keep view models small and follow the MVVM approach provided by Uno Platform.
- Use the Uno Region Navigation system where possible
