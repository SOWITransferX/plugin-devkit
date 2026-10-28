# Provider Contracts

<!-- Abgeleitet aus TransferX.Provider.Abstractions 3.0.0-preview.1 (XML-Doku und Paket-README), Stand: 2026-10-28. Sync: docs/MIGRATION.md. -->

Markdown-Referenz der Provider-Schnittstellen. Quellcode und NuGet-Paket: `TransferX.Provider.Abstractions`.

## Interfaces

### `IProvider`

| Member | Beschreibung |
| --- | --- |
| `string Name` | Anzeigename |
| `string Version` | Versionsnummer |
| `string ProviderType` | Technischer Typ (z.B. `"WebDav"`) |
| `ProviderCapability SupportedOperations` | Unterstützte Standard-Operationen. Default: alle sechs. Nur Plugins mit eingeschränktem Switch überschreiben die Property. |
| `InitializeAsync(ProviderConfigItem, CancellationToken)` | Initialisierung vor erster Operation |
| `ExecuteAsync(ProviderRequest, IProgress<FileProgress>?, CancellationToken)` | Dispatch an Commands/Queries |

**Instanziierung:** `Activator.CreateInstance` — parameterloser Konstruktor zwingend.

### `IProviderCommand<TRequest, TResponse>`

CQS-Command. Constraints: `TRequest : ProviderRequest`, `TResponse : ProviderResponse`.

### `IProviderQuery<TRequest, TResponse>`

CQS-Query. Gleiche Constraints wie Command.

## Metadata

### `[ProviderMetadata]`

Marker für Plugin-Discovery. Metadaten (`Name`, `Version`, `ProviderType`) über Interface-Properties.

## Models

| Typ | Beschreibung |
| --- | --- |
| `ProviderConfigItem` | `Id`, `Name`, `ProviderType`, `BasePath`, `Credentials?` |
| `ProviderCredentials` | `Username`, `Password` (maskiert in `ToString()`) |
| `FileItem` / `FolderItem` | Datei-/Ordner-Einträge (erben von `ContentItem`). `LastModified` ist `DateTime?`. |

## Standard-Requests

| Request | Response | Operation |
| --- | --- | --- |
| `ListFoldersRequest` | `ListFoldersResponse` | Ordner auflisten |
| `ListFilesRequest` | `ListFilesResponse` | Dateien auflisten |
| `DownloadFileRequest` | `DownloadFileResponse` | Datei herunterladen |
| `UploadFileRequest` | `UploadFileResponse` | Datei hochladen |
| `CreateFolderRequest` | `CreateFolderResponse` | Ordner erstellen |
| `DeleteFileRequest` | `DeleteFileResponse` | Datei löschen |

## Build-Sperre

`TransferX.Provider.Abstractions` verbietet eine Referenz auf `TransferX.Core` (Paket oder Projekt `TransferX.Core.csproj`). Der Build endet mit:

```text
TransferX-Plugins dürfen TransferX.Core nicht referenzieren – nur TransferX.Provider.Abstractions / TransferX.Transfer.Abstractions.
```

Version: [package-versions.json](../package-versions.json) (`3.0.0-preview.1`).

## Implementierungs-Checkliste

1. `[ProviderMetadata]` + `IProvider`
2. Parameterloser Konstruktor
3. `InitializeAsync` validiert `BasePath` und speichert Credentials
4. `ExecuteAsync` mit Typ-Switch auf alle unterstützten Requests
5. `IProgress<FileProgress>` in Commands/Queries unterstützen
6. Keine `TransferX.Core`-Referenz

Details: [docs/providers/implement-provider-plugin.md](../../docs/providers/implement-provider-plugin.md)
