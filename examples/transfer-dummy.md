# Referenz: Transfer Dummy

## Kanonisches Repository

**GitHub:** https://github.com/SOWITransferX/Transfer-Dummy

Referenz- und Test-Transfer-Plugin mit Command `MyCommand`, Stub-`ExecuteAsync` und MSTest-Tests.

## Lokaler Owner-Pfad

```text
Data\Repositories\TransferX\Source\Plugins\Transfers\Dummy
```

## Projektstruktur

```text
Transfer-Dummy/
├── src/TransferX.Transfer.Dummy/
│   ├── MyCommandCommand.cs
│   └── Helpers/          (PathMapper, ChangeDetector)
├── tests/TransferX.Transfer.Dummy.Test/
└── TransferX.Transfer.Dummy.sln
```

## Was zeigt das Repository?

- `ITransferCommand` mit `[TransferMetadata]`
- Bewusster Stub ohne echte Dateiuebertragung
- Optionale Helper (`PathMapper`, `ChangeDetector`) als Platzhalter
- MSTest-Tests mit Mock-Providern
- Projektstruktur: `src/` + `tests/`

## Wann verwenden?

| Situation | Empfehlung |
| --- | --- |
| Neues Transfer-Plugin starten | [templates/transfer-template/](../templates/transfer-template/) kopieren |
| Vollstaendige Copy-Implementierung | [docs/transfers/implement-transfer-plugin.md](../docs/transfers/implement-transfer-plugin.md) Abschnitt MyCopyCommand |
| Scaffold-Struktur verstehen | GitHub-Repo lesen |

## NuGet

Referenziert `TransferX.Transfer.Abstractions` — Version siehe [sdk/package-versions.json](../sdk/package-versions.json).

> **Hinweis:** Das GitHub-Repo kann noch `TransferX.Core` referenzieren (historisch).
> DevKit-Templates enthalten **keine** Core-Referenz — siehe [AGENTS.md](../AGENTS.md).

## Weiterführend

- [docs/transfers/implement-transfer-plugin.md](../docs/transfers/implement-transfer-plugin.md)
- [skills/transfers/SKILL.md](../skills/transfers/SKILL.md)
