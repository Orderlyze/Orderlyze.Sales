---
description: 'Implementiere ein Feature mit komplettem GitHub Issue + PR Workflow'
---
# Vollständiger Feature Workflow

Implementiere das Feature: `${input:FeatureDescription}`

## PFLICHT-SCHRITTE (in dieser Reihenfolge):

### 1. GitHub Issue erstellen
```bash
gh issue create --title "${input:FeatureDescription}" --body "Feature Request: ${input:FeatureDescription}" --label "enhancement"
```
Speichere die Issue-Nummer!

### 2. Feature Branch erstellen
```bash
git checkout -b feature/issue-[NUMMER]-[feature-name]
```

### 3. Implementierung
- Code schreiben
- ALLE hardcoded values durch Styles ersetzen
- Tests hinzufügen wenn möglich

### 4. Build & Test
```bash
dotnet build
```
Bei Fehlern: SOFORT fixen und erneut builden!

### 5. Commit & Push
```bash
git add -A
git commit -m "feat: ${input:FeatureDescription} (#[ISSUE-NUMMER])"
git push -u origin [BRANCH-NAME]
```

### 6. Pull Request
```bash
gh pr create --title "feat: ${input:FeatureDescription}" --body "Closes #[ISSUE-NUMMER]" --base master
```

WICHTIG: Führe JEDEN Befehl tatsächlich aus! Kein Simulieren!