# Transfer Contracts

<!-- Abgeleitet aus TransferX.Transfer.Abstractions 3.0.0-preview.1 (XML-Doku und Paket-README), Stand: 2026-10-28. Sync: docs/MIGRATION.md. -->

Markdown-Referenz der Transfer-Schnittstellen. Quellcode und NuGet-Paket: `TransferX.Transfer.Abstractions`.

## Interface

### `ITransferCommand`

| Member | Beschreibung |
| --- | --- |
| `string CommandName` | Eindeutiger Name (z.B. `"Copy"`) |
| `string Description` | Beschreibung |
| `string Version` | Versionsnummer |
| `ExecuteAsync(TransferContext context, CancellationToken cancellationToken)` | Transfer ausführen |

**Regel:** Genau **ein** `ITransferCommand` pro Plugin-Assembly.

Der Host setzt `TransferContext`. Plugins sehen keine Provider-IDs und erzeugen keinen `ProgressAggregator`.

## Metadata

### `[TransferMetadata]`

Marker für Plugin-Discovery via `TransferLoader`.

## Modelle

### `TransferContext`

Sealed Record. Ausführungskontext, den der Host übergibt.

| Property | Typ | Beschreibung |
| --- | --- | --- |
| `TransferId` | `Guid` | Eindeutige Transfer-ID |
| `SourcePath` | `string` | Quellpfad |
| `TargetPath` | `string` | Zielpfad |
| `Source` | `IProvider` | Bereits initialisierter Quell-Provider |
| `Target` | `IProvider` | Bereits initialisierter Ziel-Provider |
| `Progress` | `ITransferProgress` | Fortschritt des Hosts, **nie null** |

### `ITransferProgress`

Namespace `TransferX.Transfer.Abstractions.Progress`. Der Host übergibt die Implementierung (`ProgressAggregator` aus demselben Paket).

| Methode | Beschreibung |
| --- | --- |
| `Initialize(int totalFiles, long totalBytes)` | Vor dem ersten `StartFile` |
| `StartFile(string filePath, long fileSize)` | Dateiübertragung beginnt |
| `ReportFileProgress(string filePath, long transferredBytes, long totalBytes)` | Byte-Fortschritt einer laufenden Datei |
| `CompleteFile(string filePath, long fileSize)` | Datei erfolgreich abgeschlossen |
| `FailFile(string filePath, long fileSize)` | Datei fehlgeschlagen. Der zweite Parameter ist die Dateigrösse, keine Fehlermeldung. |

### `TransferResult`

Zähler und Erfolg werden berechnet. Plugins setzen `TotalFiles`, `FileResults` und optional `ErrorMessage`.

| Property | Beschreibung |
| --- | --- |
| `Success` | Berechnet: `ErrorMessage` ist null und `FailedFiles` ist 0. Nicht setzbar. |
| `TotalFiles` | Gefundene Dateien. Vom Plugin gesetzt. |
| `SuccessfulFiles` | Berechnet aus `FileResults` mit Status `Completed` |
| `FailedFiles` | Berechnet aus `FileResults` mit Status `Failed` |
| `TotalBytesTransferred` | Berechnet: Summe von `BytesTransferred` |
| `FileResults` | Detail pro Datei |
| `ErrorMessage` | Globaler Fehler, `null` bei Erfolg |

`Duration` ist kein Member von `TransferResult`.

### `FileTransferResult`

Ergebnis pro Datei: `FileName`, `SourcePath`, `Status`, `ErrorMessage`, `BytesTransferred`.

## Typischer ExecuteAsync-Ablauf

1. Dateien am Quell-Provider auflisten (`ListFilesRequest` über `context.Source`)
2. `context.Progress.Initialize` mit Anzahl und Gesamtgrösse
3. Pro Datei: `StartFile`, Download, Upload über `context.Target`, dann `CompleteFile` oder `FailFile`
4. `TransferResult` mit `TotalFiles` und `FileResults` zurückgeben
5. `OperationCanceledException` durchreichen

## Abhängigkeiten

- NuGet: **nur** `TransferX.Transfer.Abstractions` (Version aus [package-versions.json](../package-versions.json))
- **Build-Sperre:** keine `TransferX.Core`-Referenz. Der Build bricht mit der Sperrmeldung ab.

Details: [docs/transfers/implement-transfer-plugin.md](../../docs/transfers/implement-transfer-plugin.md)
