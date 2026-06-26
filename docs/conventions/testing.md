# Testing-Richtlinien für Plugins

<!-- Abgeleitet aus Dummy-Tests und Local CONTRIBUTING, Stand: 2026-06-26 -->

## Test-Framework

- **MSTest** (`Microsoft.NET.Test.Sdk`, `MSTest.TestAdapter`, `MSTest.TestFramework`)
- Testprojekt unter `tests/` neben `src/`

## Projektstruktur

```text
MyProvider/
├── src/TransferX.Provider.MyProvider/
├── tests/TransferX.Provider.MyProvider.Test/
└── TransferX.Provider.MyProvider.sln

MyCommand/
├── src/TransferX.Transfer.MyCommand/
├── tests/TransferX.Transfer.MyCommand.Test/
└── TransferX.Transfer.MyCommand.sln
```

## Provider-Tests

### Unit-Tests

- Commands/Queries isoliert testen (temporaeres Verzeichnis als `BasePath`)
- `DummyPathHelper`-Muster: Pfad-Validierung, Traversal-Schutz

### Provider-Integrationstests

- `InitializeAsync` mit gueltiger/ungueltiger Konfiguration
- `ExecuteAsync` fuer alle Request-Typen
- `ExecuteAsync` ohne vorheriges `InitializeAsync` → `InvalidOperationException`

### Discovery-Test

- Provider instanziierbar via parameterlosem Konstruktor
- `[ProviderMetadata]` vorhanden

### Beispiel-Referenz

https://github.com/SOWITransferX/Provider-Dummy — vollstaendige Test-Suite unter `tests/`.

## Transfer-Tests

- Provider per **Mock** (`Moq` optional) bereitstellen
- Metadaten testen: `CommandName`, `Description`, `Version`
- Stub-`ExecuteAsync`: leeres Erfolgsergebnis, Cancellation, Exception-Handling

### Beispiel-Referenz

https://github.com/SOWITransferX/Transfer-Dummy — vollstaendige Test-Suite unter `tests/`.

## Code-Qualitaet

- Datei-Header in jeder Testdatei
- Deutsche XML-Kommentare
- [.editorconfig](../../.editorconfig) befolgen
- `InternalsVisibleTo` fuer Tests auf interne Commands/Queries

## Build & Test

```bash
dotnet build MyProvider.sln
dotnet test MyProvider.sln
```

## Weiterführend

- [coding-standards.md](coding-standards.md)
- [../providers/implement-provider-plugin.md](../providers/implement-provider-plugin.md)
- [../transfers/implement-transfer-plugin.md](../transfers/implement-transfer-plugin.md)
