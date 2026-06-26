---
name: transferx-transfer-plugin
description: >-
  Transfer-Plugin für TransferX implementieren oder erweitern.
  Verwenden bei ITransferCommand, TransferMetadata, ExecuteAsync.
---

# TransferX Transfer Plugin Skill

## Voraussetzungen

- [skills/common/SKILL.md](../common/SKILL.md) gelesen
- [sdk/package-versions.json](../../sdk/package-versions.json)
- Leitfaden: [docs/transfers/implement-transfer-plugin.md](../../docs/transfers/implement-transfer-plugin.md)

## Template

Basis: [templates/transfer-template/](../../templates/transfer-template/) — Projektstruktur `src/` + `tests/`

Referenz (GitHub): [examples/transfer-dummy.md](../../examples/transfer-dummy.md)

## Architektur

```text
MyCommandCommand (ITransferCommand)
├── [TransferMetadata]
├── CommandName, Description, Version
└── ExecuteAsync(TransferConfigItem, sourceProvider, targetProvider, progress, ct)
```

## Pflicht-Implementierung

1. `[TransferMetadata]` auf der Command-Klasse
2. `ITransferCommand` implementieren
3. NuGet: **nur** `TransferX.Transfer.Abstractions` (Version aus `package-versions.json`)
4. **Keine** `TransferX.Core`-Referenz im Plugin-Projekt

## ExecuteAsync-Muster

- Quell- und Ziel-Provider werden vom Host injiziert
- Typischer Ablauf: `ListFiles` → `DownloadFile` → `UploadFile` über `IProvider.ExecuteAsync`
- Fortschritt über `IProgress<ProgressReport>`
- `OperationCanceledException` durchreichen
- Andere Exceptions in `TransferResult` mit `Success = false` und `ErrorMessage`

## Deployment

Kompilierte `.dll` ins Transfer-Plugin-Verzeichnis des Hosts. Siehe
[docs/architecture/plugin-lifecycle.md](../../docs/architecture/plugin-lifecycle.md).

## Checkliste

- [ ] `[TransferMetadata]` gesetzt
- [ ] Eindeutiger `CommandName`
- [ ] Keine `TransferX.Core`-PackageReference
- [ ] Fehlerbehandlung in `ExecuteAsync`
- [ ] Tests vorhanden (MSTest, Provider per Mock)
- [ ] Datei-Header und deutsche XML-Docs
