# WebApi Guidelines

All endpoints must use the Shiny Mediator pattern.

- Request classes implement `IRequest<T>` and live in `Mediator/Requests`.
- Handlers implement `IRequestHandler<TRequest, TResponse>` and live in `Mediator/Handlers`.
- Group related handlers using `[MediatorHttpGroup]` and expose actions with `[MediatorHttpGet]`, `[MediatorHttpPost]`, etc.
- Register mediator endpoints in `Program.cs` using `AddShinyMediator` and `MapGeneratedMediatorEndpoints`.
