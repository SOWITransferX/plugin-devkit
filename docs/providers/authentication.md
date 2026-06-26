# Authentifizierung in Provider-Plugins

<!-- Abgeleitet aus TransferX Abstractions und Implement-Guides, Stand: 2026-06-26 -->

Provider-Plugins erhalten Zugangsdaten ueber die Konfiguration — nicht ueber Dependency Injection im Konstruktor.

## Datenmodell

### `ProviderCredentials`

| Property | Beschreibung |
| --- | --- |
| `Username` | Benutzername |
| `Password` | Passwort |

`ToString()` maskiert das Passwort (`Username:***`).

### `ProviderConfigItem`

| Property | Beschreibung |
| --- | --- |
| `BasePath` | Basis-URL oder Pfad (provider-spezifisch) |
| `Credentials` | Optional; `null` wenn keine Authentifizierung |

## Initialisierung

Credentials werden in `InitializeAsync` entgegengenommen und fuer Commands/Queries gespeichert:

```csharp
public Task InitializeAsync(ProviderConfigItem config, CancellationToken cancellationToken = default)
{
    this._basePath = config.BasePath;
    this._credentials = config.Credentials;

    // Verbindung validieren, Token holen, etc.
    return Task.CompletedTask;
}
```

## Verwendung in Commands/Queries

Typisches Muster: Credentials als readonly-Feld an Command/Query-Konstruktor uebergeben:

```csharp
internal sealed class UploadFileCommand : IProviderCommand<UploadFileRequest, UploadFileResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    public UploadFileCommand(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }
}
```

## Best Practices

- Credentials **nicht** loggen (Passwort nie im Klartext)
- Bei fehlenden Credentials: klare Exception oder `UnauthorizedAccessException`
- Verbindungstest in `InitializeAsync` oder ueber Host-API `TestConnectionAsync`
- Provider ohne Authentifizierung: `Credentials` ignorieren (wie Dummy-Provider)

## Dummy-Referenz

Der [Provider-Dummy](https://github.com/SOWITransferX/Provider-Dummy) akzeptiert Credentials, wertet sie aber nicht aus — nur maskiertes Logging.

## Weiterführend

- [implement-provider-plugin.md](implement-provider-plugin.md)
- [../architecture/abstractions.md](../architecture/abstractions.md) — `ProviderCredentials`
- [../architecture/plugin-lifecycle.md](../architecture/plugin-lifecycle.md)
