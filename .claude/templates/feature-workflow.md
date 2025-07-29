# Standard Feature Implementation Workflow

## Checkliste für Feature-Implementierung

Bei jeder Feature-Anfrage MUSS ich:

- [ ] GitHub Issue erstellen mit `gh issue create`
- [ ] Feature Branch erstellen: `feature/issue-{number}-{name}`
- [ ] Code implementieren
- [ ] Build ausführen: `dotnet build`
- [ ] Build-Fehler beheben (wiederholen bis erfolgreich)
- [ ] Änderungen committen
- [ ] Branch pushen
- [ ] Pull Request erstellen mit `gh pr create`
- [ ] PR mit Issue verlinken

## Befehle

```bash
# 1. Issue erstellen
gh issue create --title "[Title]" --body "[Description]"

# 2. Branch erstellen
git checkout -b feature/issue-XXX-feature-name

# 3. Nach Implementierung
dotnet build

# 4. Bei Erfolg
git add -A
git commit -m "feat: [Description] (#XXX)"
git push -u origin feature/issue-XXX-feature-name

# 5. PR erstellen
gh pr create --title "feat: [Title]" --body "Closes #XXX"
```

## Wichtige Regeln

1. NIEMALS Code ändern ohne Issue
2. NIEMALS PR ohne erfolgreichen Build
3. IMMER Build-Fehler zuerst fixen
4. IMMER Issue und PR verlinken