# Transfer Template — TransferX.Transfer.MyCommand

Scaffold fuer ein neues TransferX Transfer-Plugin.

## Platzhalter ersetzen

| Platzhalter | Ersetzen durch |
| --- | --- |
| `MyCommand` | Command-Name (z.B. `Copy`, `Sync`) |
| `TransferX.Transfer.MyCommand` | Vollstaendiger Projekt-/Namespace-Name |

## NuGet-Version

Version in `.csproj`-Dateien aus [sdk/package-versions.json](../../sdk/package-versions.json) uebernehmen.

**Keine** `TransferX.Core`-Referenz — nur `TransferX.Transfer.Abstractions`.

## Struktur

```text
transfer-template/
├── src/TransferX.Transfer.MyCommand/
│   ├── MyCommandCommand.cs
│   └── Helpers/          (optional)
├── tests/TransferX.Transfer.MyCommand.Test/
└── TransferX.Transfer.MyCommand.sln
```

## Build & Test

```bash
dotnet build TransferX.Transfer.MyCommand.sln
dotnet test TransferX.Transfer.MyCommand.sln
```

## Weiterführend

- [docs/transfers/implement-transfer-plugin.md](../../docs/transfers/implement-transfer-plugin.md)
- [examples/transfer-dummy.md](../../examples/transfer-dummy.md)
- [skills/transfers/SKILL.md](../../skills/transfers/SKILL.md)
