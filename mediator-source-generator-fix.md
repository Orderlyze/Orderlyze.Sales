# MediatorHttpRequestSourceGenerator Desktop Fix

## Issue
The MediatorHttpRequestSourceGenerator was not working for the `net9.0-desktop` target in Uno projects because of a target name conflict with Uno.WinUI's build targets.

## Root Cause
- Uno.WinUI defines its own `_InjectAdditionalFiles` target in `/Uno.UI.SourceGenerators.props`
- This was overriding the `_InjectAdditionalFiles` target from Shiny.Mediator's `SourceGenerators.targets`
- As a result, MediatorHttp items were not being added as AdditionalFiles for the source generator

## Fix
Change the target name in `submodules/mediator/src/Shiny.Mediator.SourceGenerators/SourceGenerators.targets` from:
```xml
<Target Name="_InjectAdditionalFiles" BeforeTargets="GenerateMSBuildEditorConfigFileShouldRun">
```

To:
```xml
<Target Name="_InjectMediatorHttpAdditionalFiles" BeforeTargets="GenerateMSBuildEditorConfigFileShouldRun;GenerateMSBuildEditorConfigFileCore">
```

## Action Required
This fix needs to be submitted as a PR to the Shiny.Mediator repository (https://github.com/Codelisk/mediator) since it's a submodule change.

### Steps:
1. Fork the mediator repository
2. Create a branch with this fix
3. Submit a PR with the change to SourceGenerators.targets
4. Once merged, update the submodule reference in Orderlyze.Sales

## Additional Notes
- Also added `GenerateMSBuildEditorConfigFileCore` to BeforeTargets to ensure compatibility with different MSBuild versions
- This fix will make the source generator work consistently across all target frameworks in Uno projects