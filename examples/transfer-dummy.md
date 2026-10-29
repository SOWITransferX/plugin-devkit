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

> **Hinweis:** Das DevKit-Template referenziert nur `TransferX.Transfer.Abstractions` `3.0.0`.
> `ExecuteAsync` nimmt einen `TransferContext`; Fortschritt läuft über `ITransferProgress`.
> Eine `TransferX.Core`-Referenz bricht den Build ab (Build-Sperre). Das GitHub-Repo kann historisch noch Core enthalten, bis es auf dieselbe Major-Version nachgezogen ist.

## Weiterführend

- [docs/transfers/implement-transfer-plugin.md](../docs/transfers/implement-transfer-plugin.md)
- [skills/transfers/SKILL.md](../skills/transfers/SKILL.md)
