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
└── ExecuteAsync(TransferContext, cancellationToken)
```

## Pflicht-Implementierung

1. `[TransferMetadata]` auf der Command-Klasse
2. `ITransferCommand` implementieren
3. NuGet: **nur** `TransferX.Transfer.Abstractions` (Version aus `package-versions.json`, aktuell `3.0.0`)
4. **Keine** `TransferX.Core`-Referenz. Die Build-Sperre bricht den Build ab.

## ExecuteAsync-Muster

- Quell- und Ziel-Provider liegen auf `TransferContext` (`Source`, `Target`)
- Typischer Ablauf: `ListFiles` → `DownloadFile` → `UploadFile` über `context.Source` / `context.Target`
- Fortschritt über `context.Progress` (`ITransferProgress`, nie null). Tests: `Mock<ITransferProgress>`
- `OperationCanceledException` durchreichen
- Globale Fehler über `TransferResult.ErrorMessage`. `Success` und die Summen berechnet das Result.

## Deployment

Kompilierte `.dll` ins Transfer-Plugin-Verzeichnis des Hosts. Siehe
[docs/architecture/plugin-lifecycle.md](../../docs/architecture/plugin-lifecycle.md).

## Checkliste

- [ ] `[TransferMetadata]` gesetzt
- [ ] Eindeutiger `CommandName`
- [ ] Keine `TransferX.Core`-PackageReference (Build-Sperre)
- [ ] Fehlerbehandlung in `ExecuteAsync`
- [ ] Tests vorhanden (MSTest, Provider und `ITransferProgress` per Mock, Referenztest ohne Core)
- [ ] Datei-Header und deutsche XML-Docs
