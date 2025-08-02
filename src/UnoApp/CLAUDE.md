# Orderlyze.Sales Development Guidelines

## Architecture Pattern
- **MVVM Pattern**: Use Model-View-ViewModel architecture throughout the application
- **Uno.Reactive**: Leverage Uno Platform's reactive programming capabilities alongside MVVM
- **Separation of Concerns**: Keep business logic in ViewModels, UI logic in Views

## Core Principles
1. **ViewModels**
   - Inherit from appropriate base classes (e.g., `ViewModelBase`)
   - Use `INotifyPropertyChanged` for property change notifications
   - Implement commands using `ICommand` pattern
   - Keep ViewModels testable and UI-agnostic

2. **Reactive Programming**
   - Use Uno.Reactive extensions for data binding and state management
   - Leverage `Feed<T>` for data streams
   - Implement `IState<T>` for mutable state
   - See @docs/reactive-programming.md for detailed guidelines

3. **Data Binding**
   - Use x:Bind for compile-time binding where possible
   - Prefer reactive bindings over traditional MVVM bindings when appropriate
   - Ensure proper disposal of subscriptions

4. **Navigation**
   - Use dependency injection for navigation services
   - Keep navigation logic in ViewModels, not code-behind

5. **Dependency Injection**
   - Register services in `App.cs`
   - Use constructor injection in ViewModels
   - Follow the existing DI patterns in the project

## Code Style
- Use C# 12 features where beneficial
- Follow existing naming conventions in the codebase
- Keep methods focused and testable
- Write self-documenting code

## Testing
- Write unit tests for ViewModels
- Mock external dependencies
- Test reactive streams properly

## File Organization
- ViewModels: `Presentation/ViewModels/`
- Views: `Presentation/Views/`
- Models: `Business/Models/`
- Services: `Business/Services/`

## Import References
@.claude/mvvm-patterns.md
