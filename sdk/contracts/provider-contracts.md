# Provider Contracts

<!-- Abgeleitet aus TransferX\Source\TransferX\docs\TransferXAbstractions.md, Stand: 2026-06-26 -->

Markdown-Referenz der Provider-Schnittstellen. Quellcode und NuGet-Paket: `TransferX.Provider.Abstractions`.

## Interfaces

### `IProvider`

| Member | Beschreibung |
| --- | --- |
| `string Name` | Anzeigename |
| `string Version` | Versionsnummer |
| `string ProviderType` | Technischer Typ (z.B. `"WebDav"`) |
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
| `FileItem` / `FolderItem` | Datei-/Ordner-Einträge (erben von `ContentItem`) |

## Standard-Requests

| Request | Response | Operation |
| --- | --- | --- |
| `ListFoldersRequest` | `ListFoldersResponse` | Ordner auflisten |
| `ListFilesRequest` | `ListFilesResponse` | Dateien auflisten |
| `DownloadFileRequest` | `DownloadFileResponse` | Datei herunterladen |
| `UploadFileRequest` | `UploadFileResponse` | Datei hochladen |
| `CreateFolderRequest` | `CreateFolderResponse` | Ordner erstellen |
| `DeleteFileRequest` | `DeleteFileResponse` | Datei löschen |

## Implementierungs-Checkliste

1. `[ProviderMetadata]` + `IProvider`
2. Parameterloser Konstruktor
3. `InitializeAsync` validiert `BasePath` und speichert Credentials
4. `ExecuteAsync` mit Typ-Switch auf alle Requests
5. `IProgress<FileProgress>` in Commands/Queries unterstützen

Details: [docs/providers/implement-provider-plugin.md](../../docs/providers/implement-provider-plugin.md)
