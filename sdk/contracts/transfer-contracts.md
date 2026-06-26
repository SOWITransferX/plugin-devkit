# Transfer Contracts

<!-- Abgeleitet aus TransferX\Source\TransferX\docs\TransferXAbstractions.md, Stand: 2026-06-26 -->

Markdown-Referenz der Transfer-Schnittstellen. Quellcode und NuGet-Paket: `TransferX.Transfer.Abstractions`.

## Interface

### `ITransferCommand`

| Member | Beschreibung |
| --- | --- |
| `string CommandName` | Eindeutiger Name (z.B. `"Copy"`) |
| `string Description` | Beschreibung |
| `string Version` | Versionsnummer |
| `ExecuteAsync(TransferConfigItem, IProvider source, IProvider target, IProgress<ProgressReport>?, CancellationToken)` | Transfer ausführen |

**Regel:** Genau **ein** `ITransferCommand` pro Plugin-Assembly.

## Metadata

### `[TransferMetadata]`

Marker für Plugin-Discovery via `TransferLoader`.

## Models

### `TransferConfigItem`

| Property | Beschreibung |
| --- | --- |
| `TransferId` | Eindeutige Transfer-ID |
| `SourceProviderId` / `SourcePath` | Quelle |
| `TargetProviderId` / `TargetPath` | Ziel |
| `CommandName` | Name des Commands |

### `TransferResult`

| Property | Beschreibung |
| --- | --- |
| `Success` | Gesamterfolg |
| `TotalFiles`, `SuccessfulFiles`, `FailedFiles` | Dateizähler |
| `TotalBytesTransferred` | Bytes gesamt |
| `Duration` | Dauer |
| `FileResults` | Detail pro Datei |
| `ErrorMessage` | Fehler bei globalem Fehler |

### `FileTransferResult`

Ergebnis pro Datei: `FileName`, `SourcePath`, `Status`, `ErrorMessage`, `BytesTransferred`.

## Typischer ExecuteAsync-Ablauf

1. Dateien am Quell-Provider auflisten (`ListFilesRequest`)
2. Pro Datei: Download (`DownloadFileRequest`) → Upload (`UploadFileRequest`)
3. Fortschritt über `IProgress<ProgressReport>` melden
4. `OperationCanceledException` durchreichen

## Abhängigkeiten

- NuGet: **nur** `TransferX.Transfer.Abstractions`
- **Keine** `TransferX.Core`-Referenz im Plugin-Projekt

Details: [docs/transfers/implement-transfer-plugin.md](../../docs/transfers/implement-transfer-plugin.md)
