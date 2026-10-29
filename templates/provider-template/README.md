# Provider Template — TransferX.Provider.MyProvider

Scaffold fuer ein neues TransferX Provider-Plugin.

## Platzhalter ersetzen

| Platzhalter | Ersetzen durch |
| --- | --- |
| `MyProvider` | Technischer Name (z.B. `Ftp`, `WebDav`) |
| `MyProviderType` | ProviderType-String (z.B. `"Ftp"`) |
| `TransferX.Provider.MyProvider` | Vollstaendiger Projekt-/Namespace-Name |

## NuGet-Version

Version in `.csproj`-Dateien aus [sdk/package-versions.json](../../sdk/package-versions.json) uebernehmen (`3.0.0`).

**Build-Sperre:** Eine zusätzliche `PackageReference` auf `TransferX.Core` bricht den Build ab:

```text
TransferX-Plugins dürfen TransferX.Core nicht referenzieren – nur TransferX.Provider.Abstractions / TransferX.Transfer.Abstractions.
```

## Struktur

```text
provider-template/
├── src/TransferX.Provider.MyProvider/
│   ├── MyProvider.cs
│   ├── Commands/
│   └── Queries/
├── tests/TransferX.Provider.MyProvider.Test/
└── TransferX.Provider.MyProvider.sln
```

## Build & Test

```bash
dotnet build TransferX.Provider.MyProvider.sln
dotnet test TransferX.Provider.MyProvider.sln
```

## Weiterführend

- [docs/providers/implement-provider-plugin.md](../../docs/providers/implement-provider-plugin.md)
- [examples/provider-dummy.md](../../examples/provider-dummy.md)
- [skills/providers/SKILL.md](../../skills/providers/SKILL.md)
