# AGENTS.md — TransferX plugin-devkit

Master-Anleitung für alle KI-Coding-Agenten in diesem Repository.

## Lesereihenfolge

1. Diese Datei (`AGENTS.md`)
2. [skills/common/SKILL.md](skills/common/SKILL.md)
3. Je nach Aufgabe:
   - Provider: [skills/providers/SKILL.md](skills/providers/SKILL.md)
   - Transfer: [skills/transfers/SKILL.md](skills/transfers/SKILL.md)
4. [sdk/package-versions.json](sdk/package-versions.json) — kanonische NuGet-Versionen
5. [docs/](docs/) — Architektur und Implementierungsleitfäden
6. [templates/](templates/) — beim Erstellen neuer Plugins
7. [examples/](examples/) — Links zu Referenz-Repositories auf GitHub

## Harte Regeln

- **Datei-Header** in jeder C#- und CSHTML-Datei:
  ```
  SOWI Informatik, www.sowi.ch
  Franz Schönbächler
  ```
- **Kommentare und XML-Docs:** Deutsch
- **Codestil:** [.editorconfig](.editorconfig) exakt befolgen
- **NuGet-Versionen:** ausschliesslich aus [sdk/package-versions.json](sdk/package-versions.json)
- **Paketnamen:** Plural — `TransferX.Provider.Abstractions`, `TransferX.Transfer.Abstractions`
- **Kein TransferX-Core ändern** — dieses Repo ist nur für Plugin-Entwicklung
- **Keine Submodule, Symlinks oder externe Ordner-Referenzen**

## Neues Provider-Plugin erstellen

1. [templates/provider-template/](templates/provider-template/) kopieren
2. Platzhalter ersetzen: `MyProvider`, `MyProviderType`, Namespaces
3. NuGet-Referenz auf `TransferX.Provider.Abstractions` (Version aus `sdk/package-versions.json`)
4. [docs/providers/implement-provider-plugin.md](docs/providers/implement-provider-plugin.md) befolgen
5. Referenz: [examples/provider-dummy.md](examples/provider-dummy.md)

**Wichtig:** Provider-Klassen brauchen einen **parameterlosen Konstruktor** für die Plugin-Discovery.
Kein Primary Constructor als einziger Konstruktor — siehe [docs/architecture/plugin-lifecycle.md](docs/architecture/plugin-lifecycle.md).

## Neues Transfer-Plugin erstellen

1. [templates/transfer-template/](templates/transfer-template/) kopieren
2. Platzhalter ersetzen: `MyCommand`, Namespaces
3. NuGet-Referenz nur auf `TransferX.Transfer.Abstractions` (Version aus `sdk/package-versions.json`)
4. **Keine** `TransferX.Core`-Referenz im Plugin-Projekt
5. [docs/transfers/implement-transfer-plugin.md](docs/transfers/implement-transfer-plugin.md) befolgen
6. Referenz: [examples/transfer-dummy.md](examples/transfer-dummy.md)

## Dokumentations-Sync

Quelle der Wahrheit: `Data\Repositories\TransferX\Source`

Bei Aktualisierung: Dokumente **kopieren**, Links anpassen, Provenance-Kommentar setzen.
Details: [docs/MIGRATION.md](docs/MIGRATION.md)

## Verzeichnisübersicht

```text
plugin-devkit/
├── AGENTS.md              ← diese Datei
├── docs/                  ← Architektur, Leitfäden, Konventionen
├── skills/                ← Agent-Skills (common, providers, transfers)
├── templates/             ← Scaffold für neue Plugins
├── examples/              ← GitHub-Links zu Referenz-Repos
└── sdk/                   ← NuGet-Versionen + Vertragsdokumentation
```
