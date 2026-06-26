---
name: transferx-provider-plugin
description: >-
  Provider-Plugin für TransferX implementieren oder erweitern.
  Verwenden bei IProvider, Commands, Queries, ProviderMetadata, InitializeAsync.
---

# TransferX Provider Plugin Skill

## Voraussetzungen

- [skills/common/SKILL.md](../common/SKILL.md) gelesen
- [sdk/package-versions.json](../../sdk/package-versions.json)
- Leitfaden: [docs/providers/implement-provider-plugin.md](../../docs/providers/implement-provider-plugin.md)

## Template

Basis: [templates/provider-template/](../../templates/provider-template/)

Referenz (GitHub): [examples/provider-dummy.md](../../examples/provider-dummy.md)

## Architektur

```text
MyProvider (IProvider)
├── InitializeAsync(ProviderConfigItem)
├── ExecuteAsync(ProviderRequest) → switch/dispatch
├── Commands/  → IProviderCommand<TRequest, TResponse>
└── Queries/   → IProviderQuery<TRequest, TResponse>
```

## Pflicht-Implementierung

1. `[ProviderMetadata]` auf der Provider-Klasse
2. `IProvider` implementieren: `Name`, `Version`, `ProviderType`, `InitializeAsync`, `ExecuteAsync`
3. Commands: `UploadFile`, `CreateFolder`, `DeleteFile`
4. Queries: `ListFolders`, `ListFiles`, `DownloadFile`
5. NuGet: `TransferX.Provider.Abstractions` (Version aus `package-versions.json`)

## Kritischer Fallstrick: Plugin-Discovery

Der `ProviderLoader` instanziiert Plugins via `Activator.CreateInstance` — **parameterloser Konstruktor zwingend**.

**Falsch** (Primary Constructor als einziger Konstruktor):

```csharp
[ProviderMetadata]
public sealed class MyProvider(ILoggerFactory? loggerFactory = null) : IProvider
```

**Richtig:**

```csharp
[ProviderMetadata]
public sealed class MyProvider : IProvider
{
    public MyProvider() : this(null) { }

    public MyProvider(ILoggerFactory? loggerFactory) { /* ... */ }
}
```

Details: [docs/architecture/plugin-lifecycle.md](../../docs/architecture/plugin-lifecycle.md)

## Authentifizierung

Credentials über `ProviderConfigItem.Credentials` in `InitializeAsync` — siehe
[docs/providers/authentication.md](../../docs/providers/authentication.md).

## Deployment

Kompilierte `.dll` ins Provider-Plugin-Verzeichnis des Hosts. Siehe
[docs/architecture/plugin-lifecycle.md](../../docs/architecture/plugin-lifecycle.md).

## Checkliste

- [ ] Parameterloser Konstruktor vorhanden
- [ ] `[ProviderMetadata]` gesetzt
- [ ] Alle Standard-Requests unterstützt oder bewusst abgelehnt
- [ ] `InitializeAsync` validiert Konfiguration
- [ ] Tests vorhanden (MSTest)
- [ ] Datei-Header und deutsche XML-Docs
