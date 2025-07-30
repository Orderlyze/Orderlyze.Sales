# Claude Flow Workflow für Orderlyze.Sales

## Projektsetup (einmalig ausgeführt)
```bash
cd /home/daniel/Orderlyze.Sales
npx claude-flow@alpha init --force --project-name "Orderlyze.Sales"
```

## Wann welchen Befehl verwenden

### 🐝 Hive-Mind (für komplexe Features)
Verwende `hive-mind spawn` für:
- Multi-file Features
- Architektur-Änderungen
- Neue Endpoints + UI
- Features die mehrere Projekte betreffen (WebApi + UnoApp)
- Langfristige Feature-Entwicklung

```bash
npx claude-flow@alpha hive-mind spawn "[FEATURE_DESCRIPTION]" --claude
```

### ⚡ Swarm (für schnelle Tasks)
Verwende `swarm` für:
- Bug fixes
- Einzelne Datei-Änderungen
- Code-Optimierungen
- Dokumentation-Updates
- Schnelle Implementierungen

```bash
npx claude-flow@alpha swarm "[TASK_DESCRIPTION]"
```

## Standard Workflow für Orderlyze.Sales Features

### 1. Neues Feature starten
```bash
# Template für neue Features
npx claude-flow@alpha hive-mind spawn "Implement [FEATURE_NAME] for Orderlyze.Sales with:
- WebApi endpoints following Shiny.Mediator pattern
- UnoApp UI with proper MVVM and Feed patterns
- SharedModels for data transfer
- Following existing authentication and CORS setup
- Update WebApi.json and rebuild UnoApp client" --claude
```

### 2. Feature erweitern/fortsetzen
```bash
# Status checken
npx claude-flow@alpha hive-mind status

# Memory abfragen
npx claude-flow@alpha memory query "[FEATURE_NAME]" --recent

# Weiterarbeiten
npx claude-flow@alpha swarm "[ADDITIONAL_TASK]" --continue-session
```

### 3. Parallele Features (verschiedene Namespaces)
```bash
# Auth-Features
npx claude-flow@alpha hive-mind spawn "[AUTH_FEATURE]" --namespace auth --claude

# UI-Features  
npx claude-flow@alpha hive-mind spawn "[UI_FEATURE]" --namespace ui --claude

# API-Features
npx claude-flow@alpha hive-mind spawn "[API_FEATURE]" --namespace api --claude

# Performance-Features
npx claude-flow@alpha hive-mind spawn "[PERF_FEATURE]" --namespace perf --claude
```

### 4. Session Management
```bash
# Aktuelle Sessions anzeigen
npx claude-flow@alpha hive-mind status

# Spezifische Session fortsetzen
npx claude-flow@alpha hive-mind resume session-xxxxx-xxxxx

# Memory Stats
npx claude-flow@alpha memory stats
```

## Projektspezifische Templates

### WebApi Feature Template
```bash
npx claude-flow@alpha hive-mind spawn "Add [FEATURE_NAME] to Orderlyze.Sales WebApi:
- Create new MediatorHttpGroup in src/WebApi/Mediator/Handlers/[FEATURE]/
- Add Request classes in src/WebApi/Mediator/Requests/[FEATURE]/
- Follow existing authentication patterns (RequiresAuthorization = true)
- Update Program.cs if needed
- Generate updated WebApi.json
- Copy to UnoApp and rebuild client" --namespace api --claude
```

### UnoApp Feature Template
```bash
npx claude-flow@alpha hive-mind spawn "Add [FEATURE_NAME] UI to Orderlyze.Sales UnoApp:
- Create Page in src/UnoApp/UnoApp/Presentation/Pages/[FEATURE]/
- Create ViewModel with Feed pattern
- Use generated HttpRequest classes from WebApi.json
- Follow existing navigation patterns
- Add to navigation structure" --namespace ui --claude
```

### Full-Stack Feature Template
```bash
npx claude-flow@alpha hive-mind spawn "Implement complete [FEATURE_NAME] for Orderlyze.Sales:
- WebApi: Mediator handlers and requests with authentication
- SharedModels: DTOs for data transfer
- UnoApp: Pages, ViewModels, and UI following existing patterns
- Update WebApi.json generation and UnoApp client rebuild
- Follow CQRS pattern with Shiny.Mediator
- Ensure CORS and authentication work properly" --claude
```

## Best Practices für Orderlyze.Sales

### 1. Immer Projekt-Kontext mitgeben
```bash
# Gut: Spezifisch für Orderlyze.Sales
npx claude-flow@alpha swarm "Add validation to ContactsGroup.cs in Orderlyze.Sales WebApi following existing Mediator patterns"

# Schlecht: Zu generisch
npx claude-flow@alpha swarm "Add validation to API"
```

### 2. Existing Patterns erwähnen
- Shiny.Mediator für API-Requests
- UnoApp Feed pattern für ViewModels
- [SingletonHandler] Attribute in UnoApp
- WebApi.json Generation workflow
- CORS und Authentication Setup

### 3. Memory nutzen für Konsistenz
```bash
# Vor größeren Änderungen immer prüfen
npx claude-flow@alpha memory query "authentication" --recent
npx claude-flow@alpha memory query "mediator pattern" --recent
```

### 4. Monitoring bei komplexen Features
```bash
npx claude-flow@alpha swarm monitor --dashboard --real-time
```

## Entscheidungsbaum: Hive vs Swarm

```
Brauche ich mehr als eine Datei zu ändern?
├── JA → Hive-Mind
└── NEIN → Brauche ich Kontext aus vorherigen Sessions?
    ├── JA → Swarm --continue-session
    └── NEIN → Swarm

Ist es ein neues Feature?
├── JA → Hive-Mind
└── NEIN → Ist es ein Bug/Fix?
    ├── JA → Swarm
    └── NEIN → Hive-Mind (wenn unsicher)
```

## Maintenance

Diese Datei wird bei jeder Claude Flow Nutzung automatisch als Referenz verwendet.
Alle zukünftigen Features und Tasks sollen primär über Claude Flow abgewickelt werden.