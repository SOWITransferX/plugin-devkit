---
name: transferx-plugin-common
description: >-
  Gemeinsame Regeln für TransferX Plugin-Entwicklung im plugin-devkit.
  Verwenden bei jeder Plugin-Aufgabe (Provider oder Transfer).
---

# TransferX Plugin — Common Skill

## Vor dem Start

1. [AGENTS.md](../../AGENTS.md) gelesen
2. [sdk/package-versions.json](../../sdk/package-versions.json) für NuGet-Versionen
3. [.editorconfig](../../.editorconfig) befolgen

## Harte Regeln

- Datei-Header in jeder C#-Datei:
  ```
  TransferX, transferx.ch
  
  ```
- Kommentare und /// XML-Docs: **Deutsch**
- NuGet-Pakete: **Plural** — `TransferX.Provider.Abstractions`, `TransferX.Transfer.Abstractions`
- Versionen **nur** aus `sdk/package-versions.json`
- Kein TransferX-Core-Quellcode ändern
- Keine Submodule, Symlinks oder externe Ordner-Referenzen

## Workflow: Neues Plugin

1. Passendes Template aus [templates/](../../templates/) kopieren
2. Platzhalter ersetzen (Namespace, Klassennamen, CommandName/ProviderType)
3. Implementierungsleitfaden in [docs/](../../docs/) befolgen
4. Tests schreiben — siehe [docs/conventions/testing.md](../../docs/conventions/testing.md)
5. Referenz auf GitHub: [examples/](../../examples/)

## Dokumentation

| Thema | Pfad |
| --- | --- |
| Architektur | [docs/architecture/](../../docs/architecture/) |
| Konventionen | [docs/conventions/](../../docs/conventions/) |
| Plugin-Lifecycle | [docs/architecture/plugin-lifecycle.md](../../docs/architecture/plugin-lifecycle.md) |
| Sync-Regeln | [docs/MIGRATION.md](../../docs/MIGRATION.md) |

## Skill-Auswahl

- **Provider-Plugin:** [skills/providers/SKILL.md](../providers/SKILL.md)
- **Transfer-Plugin:** [skills/transfers/SKILL.md](../transfers/SKILL.md)
