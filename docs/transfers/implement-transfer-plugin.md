<!-- Angepasst an NuGet TransferX.Transfer.Abstractions 3.0.0, Stand: 2026-10-28. Sync: docs/MIGRATION.md. -->

# Transfer Plugin implementieren

> **plugin-devkit:** Templates in [templates/transfer-template/](../../templates/transfer-template/) referenzieren nur `TransferX.Transfer.Abstractions`.
> Eine zusätzliche `TransferX.Core`-Referenz bricht den Build ab (Build-Sperre im NuGet-Paket).
> Fortschritt läuft über `TransferContext.Progress` (`ITransferProgress`). Den `ProgressAggregator` erzeugt der Host; Plugins instanziieren ihn nicht.

Basis Dokumente: [README](../../README.md), [TransferX Architektur](../architecture/architecture.md), [TransferX Abstractions](../architecture/abstractions.md), [Versionierung](../conventions/versioning.md)

Dieser Leitfaden erklärt Schritt für Schritt, wie ein neues **Transfer-Plugin** für TransferX erstellt wird.  
Ein Transfer-Plugin kapselt die Logik für einen spezifischen Transfertyp (z.B. Copy, Sync, Preview) und arbeitet mit zwei Provider-Instanzen (Quelle und Ziel).

## Inhaltsverzeichnis

1. [Überblick und Architektur](#1-überblick-und-architektur)
2. [Voraussetzungen und Abhängigkeiten](#2-voraussetzungen-und-abhängigkeiten)
3. [Projektstruktur](#3-projektstruktur)
4. [Schritt-für-Schritt-Anleitung](#4-schritt-für-schritt-anleitung)
   - [4.1 Projekt erstellen](#41-projekt-erstellen)
   - [4.2 TransferMetadataAttribute setzen](#42-transfermetadataattribute-setzen)
   - [4.3 ITransferCommand implementieren](#43-itransfercommand-implementieren)
   - [4.4 Fortschritts-Tracking (ITransferProgress)](#44-fortschritts-tracking-itransferprogress)
   - [4.5 Fehlerbehandlung und TransferResult](#45-fehlerbehandlung-und-transferresult)
5. [Modelle Referenz](#5-modelle-referenz)
6. [Plugin-Discovery durch den TransferLoader](#6-plugin-discovery-durch-den-transferloader)
7. [Vollständiges Beispiel: MyCopyCommand](#7-vollständiges-beispiel-mycopycommand)
8. [Checkliste](#8-checkliste)

## 1. Überblick und Architektur

Transfer-Plugins sind eigenständige .NET-Assemblies (`.dll`), die vom `TransferLoader` im Core Layer dynamisch geladen werden.  
Ein Plugin implementiert das `ITransferCommand`-Interface aus `TransferX.Transfer.Abstractions` und wird über das `[TransferMetadata]`-Attribut für die automatische Discovery markiert.

> **Wichtig:** Pro Assembly wird genau **ein** Transfer-Command erwartet. Ein Plugin = ein Command.

Die `TransferEngine` lädt den Command anhand von `config.CommandName` aus einem **Plugin-Verzeichnis** (nicht aus einem einzelnen Assembly-Pfad).  
Optional kann pro Transfer-Konfiguration ein eigener `PluginPath` gesetzt werden (siehe [Abschnitt 6](#6-plugin-discovery-durch-den-transferloader)).

```mermaid
graph TD
    subgraph Core["⚙️ Core"]
        TL["TransferLoader"]
        TE["TransferEngine"]
        PE["ProviderEngine"]
    end

    subgraph Abstractions["📦 TransferX.Transfer.Abstractions"]
        ITC["ITransferCommand"]
        META["TransferMetadataAttribute"]
        TCtx["TransferContext"]
        TR["TransferResult"]
        Prog["ITransferProgress"]
    end

    subgraph ProviderAbstractions["📦 TransferX.Provider.Abstractions"]
        IProvider["IProvider"]
    end

    subgraph Plugin["🔌 TransferX.Transfer.MyCopy (Neues Plugin)"]
        MyCopy["MyCopyCommand"]
    end

    TL -->|"scannt Assembly"| META
    TL -->|"instanziiert"| MyCopy
    TE -->|"CreateCommandByName"| TL
    TE -->|"erzeugt"| TCtx
    TE -->|"setzt"| Prog
    TE -->|"ruft auf"| ITC
    MyCopy -->|"implementiert"| ITC
    MyCopy -->|"nutzt"| IProvider
    MyCopy -->|"meldet über"| Prog
    Prog -->|"liegt auf"| TCtx
```

### Ablauf: Transfer-Plugin Aufruf

```mermaid
sequenceDiagram
    participant TE as TransferEngine
    participant TL as TransferLoader
    participant PE as ProviderEngine
    participant MC as MyCopyCommand
    participant SRC as IProvider (Quelle)
    participant TGT as IProvider (Ziel)
    participant Prog as ITransferProgress

    TE->>TE: ResolvePluginPath(PluginPath, globalPluginDir)
    TE->>TL: CreateCommandByName(pluginDir, CommandName)
    TL-->>TE: ITransferCommand (MyCopyCommand)
    TE->>PE: LoadAndInitializeProviderAsync(SourceProviderId)
    PE-->>TE: IProvider (Quelle)
    TE->>PE: LoadAndInitializeProviderAsync(TargetProviderId)
    PE-->>TE: IProvider (Ziel)
    TE->>TE: ProgressAggregator als ITransferProgress erzeugen
    TE->>MC: ExecuteAsync(TransferContext, ct)
    MC->>SRC: ExecuteAsync(ListFilesRequest, progress: null, ct)
    SRC-->>MC: ListFilesResponse
    MC->>Prog: Initialize(count, totalBytes)
    loop je Datei
        MC->>Prog: StartFile(path, size)
        MC->>SRC: ExecuteAsync(DownloadFileRequest, progress: null, ct)
        SRC-->>MC: DownloadFileResponse
        MC->>TGT: ExecuteAsync(UploadFileRequest, progress: null, ct)
        TGT-->>MC: UploadFileResponse
        MC->>Prog: CompleteFile oder FailFile
    end
    MC-->>TE: TransferResult
```

## 2. Voraussetzungen und Abhängigkeiten

Das Plugin-Projekt benötigt genau eine TransferX-Referenz: `TransferX.Transfer.Abstractions`.  
`TransferX.Domain` und `TransferX.Provider.Abstractions` kommen transitiv.  
`TransferX.Core` ist kein Plugin-Paket. Die Build-Sperre im Abstractions-Paket bricht den Build ab, sobald `TransferX.Core` als Paket oder als Projekt `TransferX.Core.csproj` referenziert wird.

```text
TransferX-Plugins dürfen TransferX.Core nicht referenzieren – nur TransferX.Provider.Abstractions / TransferX.Transfer.Abstractions.
```

Version aus [sdk/package-versions.json](../../sdk/package-versions.json): **3.0.0**. Host und Plugin verwenden dieselbe Major-Version, siehe [Versionierung](../conventions/versioning.md).

```mermaid
graph LR
    Plugin["🔌 TransferX.Transfer.MyCopy"]
    TA["📦 TransferX.Transfer.Abstractions"]
    PA["📦 TransferX.Provider.Abstractions"]
    Domain["💎 TransferX.Domain"]

    Plugin -->|"Pflicht"| TA
    TA -->|"transitiv"| PA
    TA -->|"transitiv"| Domain
```

**`.csproj` Minimal-Konfiguration:**  
Beispiel: TransferX.Transfer.MyCopy\TransferX.Transfer.MyCopy.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <AssemblyName>TransferX.Transfer.MyCopy</AssemblyName>
    <RootNamespace>TransferX.Transfer.MyCopy</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <!-- Version aus sdk/package-versions.json. Keine TransferX.Core-Referenz. -->
    <PackageReference Include="TransferX.Transfer.Abstractions" Version="3.0.0" />
  </ItemGroup>

</Project>
```

## 3. Projektstruktur

Empfohlene Ordnerstruktur für ein neues Transfer-Plugin:

```tex
TransferX.Transfer.MyCopy
│   MyCopyCommand.cs               ← ITransferCommand Implementierung (Haupt-Einstiegspunkt)
│   TransferX.Transfer.MyCopy.csproj
│
└───Helpers
        ChangeDetector.cs          ← Hilfklasse z.B. für Änderungserkennung (optional)
        PathMapper.cs              ← Hilfklasse für Pfad-Mapping (optional)
```

> **Hinweis:** Da pro Plugin genau ein Command implementiert wird, ist keine Commands/Queries-Unterstruktur wie beim Provider-Plugin nötig. Hilfklassen können in einem `Helpers`-Ordner organisiert werden.

## 4. Schritt-für-Schritt-Anleitung

### 4.1 Projekt erstellen

1. Neues **Class Library (.NET 8)** Projekt erstellen: `TransferX.Transfer.MyCopy`
2. Referenz auf `TransferX.Transfer.Abstractions` hinzufügen (siehe [Abschnitt 2](#2-voraussetzungen-und-abhängigkeiten))
3. `TransferX.Core` nicht referenzieren. Die Build-Sperre weist die Referenz zurück.

### 4.2 TransferMetadataAttribute setzen

Das `[TransferMetadata]`-Attribut markiert die Klasse für die automatische Plugin-Discovery durch den `TransferLoader`.  
**Ohne dieses Attribut wird das Plugin nicht gefunden und nicht geladen.**

```c#
TransferX.Transfer.MyCopy\MyCopyCommand.cs
// TransferX, transferx.ch

using TransferX.Transfer.Abstractions;
using TransferX.Transfer.Abstractions.Metadata;

namespace TransferX.Transfer.MyCopy;

/// <summary>
/// Copy-Transfer-Command-Implementierung für TransferX.<br/>
/// Kopiert alle Dateien vom Quell-Provider zum Ziel-Provider.
/// </summary>
[TransferMetadata]
public sealed class MyCopyCommand : ITransferCommand
{
    // ...
}
```

### 4.3 ITransferCommand implementieren

`ITransferCommand` schreibt drei Properties und eine Methode vor:

| Member | Pflicht | Beschreibung |
|---|---|---|
| `string CommandName` | ✅ | Eindeutiger Command-Name, z.B. `"MyCopy"` – muss systemweit eindeutig sein |
| `string Description` | ✅ | Kurzbeschreibung der Funktion, z.B. `"Kopiert alle Dateien von Quelle nach Ziel"` |
| `string Version` | ✅ | Versionsnummer, z.B. `"1.0.0"` |
| `ExecuteAsync(TransferContext, CancellationToken)` | ✅ | Transfer-Logik implementieren und `TransferResult` zurückgeben |

> **Wichtig:** `CommandName` muss systemweit eindeutig sein. Der `TransferLoader` sucht anhand dieses Strings das richtige Plugin im Plugin-Verzeichnis.

**Signatur von `ExecuteAsync`:**

```c#
Task<TransferResult> ExecuteAsync(
    TransferContext context,
    CancellationToken cancellationToken);
```

| Parameter | Typ | Beschreibung |
|---|---|---|
| `context` | `TransferContext` | Pfade, bereits initialisierte Provider und Fortschritt. `Progress` ist nie null. |
| `cancellationToken` | `CancellationToken` | Abbruch-Token für kooperativen Abbruch |

Der Host löst Provider-IDs auf. `TransferContext` enthält `Source`, `Target`, `SourcePath`, `TargetPath`, `TransferId` und `Progress` — keine Provider-IDs.

> **Hinweis `IProvider.ExecuteAsync`:** Die Signatur lautet `(ProviderRequest request, IProgress<FileProgress>? progress, CancellationToken cancellationToken)`.  
> Der zweite Parameter ist der Byte-Fortschritt auf Provider-Ebene. Listing, Download und Upload im Transfer-Command erhalten in der Regel `progress: null`. Den Transfer-Fortschritt meldet das Plugin über `context.Progress`.

### 4.4 Fortschritts-Tracking (ITransferProgress)

Die `TransferEngine` erzeugt den `ProgressAggregator` (Namespace `TransferX.Transfer.Abstractions.Progress`) und legt ihn als `ITransferProgress` auf den `TransferContext`.  
Plugins erzeugen den Aggregator nicht selbst und referenzieren `TransferX.Core` nicht.

```c#
// TransferX, transferx.ch

// Nach dem Listing, vor der ersten Datei:
context.Progress.Initialize(listResponse.Items.Count, listResponse.TotalSizeBytes);

foreach (var file in listResponse.Items)
{
    context.Progress.StartFile(file.Path, file.Size);

    try
    {
        // Datei übertragen ...
        context.Progress.ReportFileProgress(file.Path, transferredBytes, file.Size);
        context.Progress.CompleteFile(file.Path, uploadResponse.BytesTransferred);
    }
    catch (Exception)
    {
        context.Progress.FailFile(file.Path, file.Size);
        // lokal abfangen und mit FileTransferStatus.Failed fortfahren, oder weiterwerfen
    }
}
```

| Methode | Bedeutung |
|---|---|
| `Initialize(totalFiles, totalBytes)` | Einmal vor dem ersten `StartFile` |
| `StartFile(filePath, fileSize)` | Beginn einer Datei |
| `ReportFileProgress(filePath, transferredBytes, totalBytes)` | Laufender Byte-Fortschritt |
| `CompleteFile(filePath, fileSize)` | Datei erfolgreich |
| `FailFile(filePath, fileSize)` | Datei fehlgeschlagen. Der zweite Parameter ist die Dateigrösse, keine Meldung. |

Die Fehlermeldung einer Datei steht auf `FileTransferResult.ErrorMessage`, nicht auf `FailFile`.

### 4.5 Fehlerbehandlung und TransferResult

- **Einzeldatei-Fehler**: Pro Datei einen `FileTransferResult` mit `FileTransferStatus.Failed` erstellen – keinen Abbruch des gesamten Transfers. Zusätzlich `context.Progress.FailFile`.
- **Globaler Fehler** (z.B. Verbindungsabbruch): `TransferResult` mit `ErrorMessage` zurückgeben. `Success` wird daraus berechnet und ist nicht setzbar.
- **Abbruch**: `CancellationToken` prüfen und `OperationCanceledException` propagieren lassen.

`SuccessfulFiles`, `FailedFiles` und `TotalBytesTransferred` berechnet `TransferResult` aus `FileResults`. Plugins setzen `TotalFiles`, `FileResults` und bei einem globalen Fehler `ErrorMessage`.

```c#
// TransferX, transferx.ch

using TransferX.Domain.Enumerations;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;
using TransferX.Transfer.Abstractions;
using TransferX.Transfer.Abstractions.Models;

public async Task<TransferResult> ExecuteAsync(
    TransferContext context,
    CancellationToken cancellationToken)
{
    var fileResults = new List<FileTransferResult>();

    try
    {
        cancellationToken.ThrowIfCancellationRequested();

        var listResponse = (ListFilesResponse)await context.Source.ExecuteAsync(
            new ListFilesRequest { Path = context.SourcePath, Recursive = true },
            progress: null,
            cancellationToken);

        context.Progress.Initialize(listResponse.Items.Count, listResponse.TotalSizeBytes);

        foreach (var file in listResponse.Items)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Datei herunterladen und hochladen
                fileResults.Add(new FileTransferResult
                {
                    FileName = file.Name,
                    SourcePath = file.Path,
                    Status = FileTransferStatus.Completed,
                    BytesTransferred = file.Size
                });
                context.Progress.CompleteFile(file.Path, file.Size);
            }
            catch (Exception ex)
            {
                fileResults.Add(new FileTransferResult
                {
                    FileName = file.Name,
                    SourcePath = file.Path,
                    Status = FileTransferStatus.Failed,
                    ErrorMessage = ex.Message,
                    BytesTransferred = 0
                });
                context.Progress.FailFile(file.Path, file.Size);
            }
        }

        return new TransferResult
        {
            TotalFiles = listResponse.Items.Count,
            FileResults = fileResults.AsReadOnly()
        };
    }
    catch (OperationCanceledException)
    {
        throw;
    }
    catch (Exception ex)
    {
        return new TransferResult
        {
            TotalFiles = fileResults.Count,
            FileResults = fileResults.AsReadOnly(),
            ErrorMessage = ex.Message
        };
    }
}
```

## 5. Modelle Referenz

```mermaid
graph TD
    TCtx["TransferContext (sealed record)"]
    Prog["ITransferProgress"]
    TR["TransferResult (sealed record)"]
    FTR["FileTransferResult (sealed record)"]
    CI["ChangeItem (sealed record)"]
    FTS["FileTransferStatus (enum, Domain)"]
    SCT["SyncChangeType (enum)"]

    TCtx -->|"Progress"| Prog
    TR -->|"enthält Liste von"| FTR
    FTR -->|"Status"| FTS
    CI -->|"ChangeType"| SCT
```

### `TransferContext` – Ausführungskontext

Wird in `ExecuteAsync` übergeben. Der Host hat die Provider bereits initialisiert und den Fortschritt erzeugt.

| Property | Typ | Beschreibung |
|---|---|---|
| `TransferId` | `Guid` | Eindeutige ID des Transfer-Auftrags |
| `SourcePath` | `string` | Quellpfad beim Quell-Provider |
| `TargetPath` | `string` | Zielpfad beim Ziel-Provider |
| `Source` | `IProvider` | Quell-Provider |
| `Target` | `IProvider` | Ziel-Provider |
| `Progress` | `ITransferProgress` | Fortschritt, nie null |

> **Hinweis Host-Konfiguration:** Provider-IDs und der optionale Plugin-Pfad (`PluginPath`) bleiben auf der Host-Seite (`TransferConfigDto`). Die `TransferEngine` löst sie vor `ExecuteAsync` auf. Sie sind nicht Teil von `TransferContext`.

### `TransferResult` – Gesamtergebnis

| Property | Typ | Setzbar | Beschreibung |
|---|---|---|---|
| `Success` | `bool` | nein | `ErrorMessage` ist null und `FailedFiles` ist 0 |
| `TotalFiles` | `int` | ja | Anzahl der gefundenen Dateien |
| `SuccessfulFiles` | `int` | nein | Anzahl `FileResults` mit Status `Completed` |
| `FailedFiles` | `int` | nein | Anzahl `FileResults` mit Status `Failed` |
| `TotalBytesTransferred` | `long` | nein | Summe von `BytesTransferred` |
| `FileResults` | `IReadOnlyList<FileTransferResult>` | ja | Detailergebnisse je Datei |
| `ErrorMessage` | `string?` | ja | Globale Fehlermeldung, `null` bei Erfolg |

### `FileTransferResult` – Einzeldatei-Ergebnis

| Property | Typ | Beschreibung |
|---|---|---|
| `FileName` | `string` | Name der übertragenen Datei |
| `SourcePath` | `string` | Vollständiger Quellpfad der Datei |
| `Status` | `FileTransferStatus` | Endstatus der Dateiübertragung (`TransferX.Domain.Enumerations`) |
| `ErrorMessage` | `string?` | Fehlermeldung bei fehlgeschlagener Übertragung, `null` bei Erfolg |
| `BytesTransferred` | `long` | Anzahl der tatsächlich übertragenen Bytes |

### `FileTransferStatus` – Statuswerte

Enumeration aus `TransferX.Domain.Enumerations` (transitiv über `TransferX.Transfer.Abstractions` verfügbar).

| Wert | Beschreibung |
|---|---|
| `Completed` | Datei erfolgreich übertragen |
| `Failed` | Datei-Übertragung fehlgeschlagen |
| `Skipped` | Datei wurde übersprungen (z.B. beim Sync unverändert) |

> **Hinweis:** Weitere Werte (`Pending`, `Transferring`) werden primär in der Domain-Entität `TransferItem` für laufende Transfers verwendet. In `FileTransferResult` nach Abschluss einer Datei sind `Completed`, `Failed` und `Skipped` üblich.

### `ChangeItem` – Erkannte Änderung (für Sync-artige Transfers)

| Property | Typ | Beschreibung |
|---|---|---|
| `ChangeType` | `SyncChangeType` | Art der Änderung (`Added`, `Modified`, `Deleted`) |
| `SourcePath` | `string` | Quellpfad der Datei |
| `TargetPath` | `string` | Zielpfad der Datei |
| `FileSize` | `long?` | Dateigrösse in Bytes, `null` bei gelöschten Dateien |
| `LastModified` | `DateTime?` | Zeitpunkt der letzten Änderung (UTC), `null` bei Löschen |

## 6. Plugin-Discovery durch den TransferLoader

Der `TransferLoader` im Core Layer scannt Plugin-Assemblies automatisch nach Klassen, die:

1. Das `[TransferMetadata]`-Attribut tragen
2. `ITransferCommand` implementieren
3. Einen parameterlosen Konstruktor besitzen (Instanziierung via `Activator.CreateInstance`)

> **Wichtig:** Pro Assembly ist genau **ein** Transfer-Command vorgesehen. Wird mehr als ein passender Typ gefunden, gilt der **erste** gefundene Export-Typ.

| Methode | Beschreibung |
|---|---|
| `LoadCommand(assemblyPath)` | Lädt eine Assembly und gibt `TransferPluginInfo` zurück (wirft, wenn kein Command gefunden) |
| `CreateCommand(assemblyPath)` | Instanz aus einer bestimmten Assembly |
| `CreateCommandByName(pluginDirectory, commandName)` | Sucht im Verzeichnis nach passendem `CommandName` (von der `TransferEngine` verwendet) |
| `DiscoverAll(pluginDirectory)` | Durchsucht **rekursiv** alle `*.dll` im Verzeichnis und Unterverzeichnissen; Assemblies ohne gültigen Command werden still übersprungen |

`DiscoverAll` verhält sich analog zum `ProviderLoader`: Jede `*.dll` wird per Reflection geprüft. Enthält eine Assembly keinen Typ mit `[TransferMetadata]` und `ITransferCommand`, wird sie übersprungen (Log-Level `Debug`) – **ohne** Dateinamen-Filter. So funktionieren auch Third-Party-Plugins mit beliebigem Assembly-Namen. Warnungen erscheinen nur bei echten Lade- oder Instanziierungsfehlern.

```mermaid
graph LR
    TL["TransferLoader"]
    TLCtx["PluginLoadContext (isoliert)"]
    DLL["TransferX.Transfer.MyCopy.dll"]
    REFL["Reflection (GetExportedTypes)"]
    META["TransferMetadataAttribute"]
    ITCMD["ITransferCommand"]
    INFO["TransferPluginInfo"]

    TL -->|"erstellt"| TLCtx
    TLCtx -->|"lädt"| DLL
    DLL -->|"scannt"| REFL
    REFL -->|"prüft auf"| META
    REFL -->|"prüft auf"| ITCMD
    REFL -->|"erzeugt"| INFO
    INFO -->|"zurück an"| TL
```

### Plugin-Verzeichnis und `PluginPath`

Der Host konfiguriert das globale Transfer-Plugin-Verzeichnis in `appsettings.json`:

```json
"TransferX": {
  "TransferPluginDir": "C:\\Data\\TransferX\\Plugins\\Transfers"
}
```

(Schlüssel: `TransferX:TransferPluginDir` – Registrierung über `CoreServiceExtensions.AddTransferXCore`.)

Die `TransferEngine` löst den effektiven Pfad vor `CreateCommandByName` auf:

| Eingabe `PluginPath` (in `TransferConfigDto`) | Ergebnis |
|---|---|
| `null` / leer | Globales `TransferPluginDir` |
| Absoluter Pfad | Wird direkt verwendet |
| Relativer Pfad / Unterverzeichnis | Kombination mit globalem `TransferPluginDir` |

### Deployment

Die fertig kompilierte Plugin-Assembly (`.dll`) inkl. aller **eigenen** Abhängigkeiten muss im aufgelösten Plugin-Verzeichnis abgelegt werden:

```tex
{TransferPluginDir}/
    TransferX.Transfer.MyCopy.dll
    MeineHilfsBibliothek.dll       ← eigene Abhängigkeiten des Plugins
    unterordner/
        WeiteresPlugin.dll         ← DiscoverAll durchsucht Unterverzeichnisse rekursiv
```

### Shared Assemblies (TransferX.*.dll)

Der Host stellt `TransferX.Transfer.Abstractions.dll`, `TransferX.Provider.Abstractions.dll` und `TransferX.Domain.dll` über den `PluginLoadContext` bereit und teilt diese mit allen Plugins. `TransferX.Core.dll` ist kein Plugin-Vertrag und wird vom Plugin nicht referenziert.

In der Praxis kopiert `dotnet publish` oder ein Build-Skript die SDK-DLLs oft **mit** ins Plugin-Verzeichnis. Das ist unkritisch: `DiscoverAll` scannt zwar alle `*.dll`, erkennt in Shared-Assemblies aber keinen gültigen Transfer-Command und überspringt sie still. Es ist **kein** Dateinamen-Filter nötig. Die mitgelieferte SDK-Assembly muss dieselbe Major-Version wie der Host haben.

| Situation | Verhalten |
|---|---|
| Nur die Plugin-Assembly deployt | Empfohlen – weniger Dateien im Verzeichnis |
| SDK-DLLs liegen zusätzlich im Ordner | Unkritisch, solange die Major-Version zum Host passt |
| Third-Party-Plugin mit eigenem Namen | Wird über `[TransferMetadata]` + `ITransferCommand` erkannt |

## 7. Vollständiges Beispiel: MyCopyCommand

Das folgende Beispiel kopiert alle Dateien vom Quell-Provider zum Ziel-Provider. Fortschritt läuft über `context.Progress`.

```c#
TransferX.Transfer.MyCopy\MyCopyCommand.cs
// TransferX, transferx.ch

using TransferX.Domain.Enumerations;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;
using TransferX.Transfer.Abstractions;
using TransferX.Transfer.Abstractions.Metadata;
using TransferX.Transfer.Abstractions.Models;

namespace TransferX.Transfer.MyCopy;

/// <summary>
/// Copy-Transfer-Command-Implementierung für TransferX.<br/>
/// Kopiert alle Dateien rekursiv vom Quell-Provider zum Ziel-Provider.
/// </summary>
[TransferMetadata]
public sealed class MyCopyCommand : ITransferCommand
{
    /// <inheritdoc/>
    public string CommandName => "MyCopy";

    /// <inheritdoc/>
    public string Description => "Kopiert alle Dateien rekursiv vom Quell- zum Ziel-Provider.";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <inheritdoc/>
    public async Task<TransferResult> ExecuteAsync(
        TransferContext context,
        CancellationToken cancellationToken)
    {
        var fileResults = new List<FileTransferResult>();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var listResponse = (ListFilesResponse)await context.Source.ExecuteAsync(
                new ListFilesRequest { Path = context.SourcePath, Recursive = true },
                progress: null,
                cancellationToken);

            int totalFiles = listResponse.Items.Count;
            context.Progress.Initialize(totalFiles, listResponse.TotalSizeBytes);

            foreach (var file in listResponse.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string relativePath = Path.GetRelativePath(context.SourcePath, file.Path);
                string targetFilePath = Path.Combine(context.TargetPath, relativePath)
                    .Replace('\\', '/');

                context.Progress.StartFile(file.Path, file.Size);

                DownloadFileResponse? downloadResponse = null;
                try
                {
                    downloadResponse = (DownloadFileResponse)await context.Source.ExecuteAsync(
                        new DownloadFileRequest { Path = file.Path },
                        progress: null,
                        cancellationToken);

                    var uploadResponse = (UploadFileResponse)await context.Target.ExecuteAsync(
                        new UploadFileRequest
                        {
                            TargetPath = targetFilePath,
                            ContentFactory = _ => Task.FromResult(downloadResponse.Stream),
                            FileSize = downloadResponse.FileSize,
                            ContentType = downloadResponse.ContentType
                        },
                        progress: null,
                        cancellationToken);

                    fileResults.Add(new FileTransferResult
                    {
                        FileName = file.Name,
                        SourcePath = file.Path,
                        Status = uploadResponse.Success
                            ? FileTransferStatus.Completed
                            : FileTransferStatus.Failed,
                        ErrorMessage = uploadResponse.Success ? null : "Upload fehlgeschlagen.",
                        BytesTransferred = uploadResponse.BytesTransferred
                    });

                    if (uploadResponse.Success)
                    {
                        context.Progress.CompleteFile(file.Path, uploadResponse.BytesTransferred);
                    }
                    else
                    {
                        context.Progress.FailFile(file.Path, file.Size);
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    context.Progress.FailFile(file.Path, file.Size);

                    fileResults.Add(new FileTransferResult
                    {
                        FileName = file.Name,
                        SourcePath = file.Path,
                        Status = FileTransferStatus.Failed,
                        ErrorMessage = ex.Message,
                        BytesTransferred = 0
                    });
                }
                finally
                {
                    if (downloadResponse?.Stream is not null)
                    {
                        await downloadResponse.Stream.DisposeAsync();
                    }
                }
            }

            return new TransferResult
            {
                TotalFiles = totalFiles,
                FileResults = fileResults.AsReadOnly()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new TransferResult
            {
                TotalFiles = fileResults.Count,
                FileResults = fileResults.AsReadOnly(),
                ErrorMessage = ex.Message
            };
        }
    }
}
```

## 8. Checkliste

Vor dem Deployment des neuen Transfer-Plugins alle Punkte prüfen:

| # | Aufgabe | Erledigt |
|---|---|---|
| 1 | `[TransferMetadata]`-Attribut an der Command-Klasse gesetzt | ☐ |
| 2 | `ITransferCommand` vollständig implementiert (`CommandName`, `Description`, `Version`, `ExecuteAsync`) | ☐ |
| 3 | `CommandName` ist systemweit eindeutig (z.B. `"MyCopy"`, `"MySync"`) | ☐ |
| 4 | Pro Assembly genau ein `ITransferCommand` implementiert | ☐ |
| 5 | `TransferResult` setzt `TotalFiles` und `FileResults`; `Success` und die Summen bleiben berechnet | ☐ |
| 6 | `FileTransferResult` für jede verarbeitete Datei erstellt | ☐ |
| 7 | Einzeldatei-Fehler werden abgefangen und als `FileTransferStatus.Failed` protokolliert (kein Abbruch des Gesamt-Transfers) | ☐ |
| 8 | `OperationCanceledException` wird nicht abgefangen, sondern propagiert | ☐ |
| 9 | `CancellationToken` wird in allen asynchronen Aufrufen weitergereicht | ☐ |
| 10 | Fortschritt über `context.Progress` (`ITransferProgress`) gemeldet, kein eigener `ProgressAggregator` | ☐ |
| 11 | `IProvider.ExecuteAsync` mit `progress: null` für Listing/Download/Upload im Transfer-Command aufgerufen | ☐ |
| 12 | `DownloadFileResponse.Stream` wird geschlossen | ☐ |
| 13 | Zielpfade werden korrekt aus Quellpfaden berechnet (relative Struktur) | ☐ |
| 14 | Datei-Header in jeder `.cs`-Datei vorhanden | ☐ |
| 15 | XML-Kommentare (`///`) für alle `public` Members vorhanden | ☐ |
| 16 | Plugin-Assembly inkl. eigener Abhängigkeiten im Plugin-Verzeichnis deployt | ☐ |
| 17 | Keine `TransferX.Core`-Referenz. SDK-DLLs (`TransferX.*.Abstractions`, `TransferX.Domain`) teilt der Host; dieselbe Major-Version | ☐ |
