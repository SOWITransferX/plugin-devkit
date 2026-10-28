<!-- DevKit-Konvention, Stand: 2026-10-28. Abgestimmt auf die NuGet-Pakete TransferX.* 3.0.0-preview.1. Nicht aus TransferX.Core kopiert; Sync-Regeln siehe docs/MIGRATION.md. -->

# Versionierung der SDK-Pakete

Die Plugin-SDK-Pakete folgen [Semantic Versioning](https://semver.org/lang/de/).

| Paket | Rolle |
| --- | --- |
| `TransferX.Domain` | Domain-Modelle, Enumerationen, Value Objects |
| `TransferX.Provider.Abstractions` | Provider-Vertrag (`IProvider`, Commands, Queries) |
| `TransferX.Transfer.Abstractions` | Transfer-Vertrag (`ITransferCommand`, `TransferContext`, `ITransferProgress`) |

`TransferX.Core` ist **kein Plugin-Paket**. Plugins referenzieren es nicht. Die Abstractions-Pakete enthalten eine Build-Sperre, die eine `PackageReference` oder `ProjectReference` auf `TransferX.Core` mit einem Build-Fehler abbricht.

## SemVer

| Stelle | Bedeutung für das SDK |
| --- | --- |
| Major | Brechende Änderung am Plugin-Vertrag (Signatur, entfernte Member, geänderte Semantik). |
| Minor | Rückwärtskompatible Ergänzung. |
| Patch | Fehlerkorrektur ohne Vertragsbruch. |
| Preview | Vorschau, z. B. `3.0.0-preview.1`. Kein stabiles Release. |

Die kanonische Version steht in [sdk/package-versions.json](../../sdk/package-versions.json). Templates und Plugin-Projekte übernehmen diese Version wörtlich.

Aktueller Stand: **3.0.0-preview.1**. Ein späterer Stand `3.0.0` ist ein eigener Versionswechsel und kein Ersatz für diese Preview.

## Gleiche Major-Version

Host und Plugins verwenden **dieselbe Major-Version** der drei SDK-Pakete.

- Ein Host mit SDK 3.x lädt Plugins, die gegen SDK 3.x gebaut sind.
- Die drei Pakete werden gemeinsam versioniert. Ein Plugin mischt keine unterschiedlichen Major-Versionen von Domain und Abstractions.
- Der Host stellt `TransferX.Domain.dll`, `TransferX.Provider.Abstractions.dll` und `TransferX.Transfer.Abstractions.dll` über den `PluginLoadContext` bereit. Das Plugin liefert keine eigene Kopie einer anderen Major-Version mit.

Die Plugin-eigene `Version` (`IProvider.Version`, `ITransferCommand.Version`) ist unabhängig von der SDK-Version. Sie beschreibt das Plugin, nicht den Vertrag.

## Build-Sperre

Beide Abstractions-Pakete importieren ein MSBuild-Target (`TransferXForbidCoreReference`). Der Build bricht ab, wenn das Projekt `TransferX.Core` als Paket oder als Projekt `TransferX.Core.csproj` referenziert:

```text
TransferX-Plugins dürfen TransferX.Core nicht referenzieren – nur TransferX.Provider.Abstractions / TransferX.Transfer.Abstractions.
```

Fortschritt melden Transfer-Plugins über `TransferContext.Progress` (`ITransferProgress`). Den `ProgressAggregator` erzeugt der Host.

## Weiterführend

- [sdk/package-versions.json](../../sdk/package-versions.json)
- [sdk/README.md](../../sdk/README.md)
- [plugin-lifecycle.md](../architecture/plugin-lifecycle.md)
