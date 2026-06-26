# Referenz: Provider Dummy

## Kanonisches Repository

**GitHub:** https://github.com/SOWITransferX/Provider-Dummy

Vollstaendige Referenzimplementierung mit Tests, Build-Skripten und README.

## Lokaler Owner-Pfad

```text
Data\Repositories\TransferX\Source\Plugins\Providers\Dummy
```

## Was zeigt das Repository?

- `IProvider`-Implementierung mit allen Standard-Commands und -Queries
- Lokales Basisverzeichnis (`BasePath`) als Datenspeicher
- MSTest-Suite (Unit + Integration)
- Projektstruktur: `src/` + `tests/`

## Wann verwenden?

| Situation | Empfehlung |
| --- | --- |
| Neues Provider-Plugin starten | [templates/provider-template/](../templates/provider-template/) kopieren |
| Implementierungsmuster verstehen | GitHub-Repo lesen oder klonen |
| Pfad-Validierung, Tests | Dummy-Tests als Vorlage |

## NuGet

Referenziert `TransferX.Provider.Abstractions` — Version siehe [sdk/package-versions.json](../sdk/package-versions.json).

## Weiterführend

- [docs/providers/implement-provider-plugin.md](../docs/providers/implement-provider-plugin.md)
- [skills/providers/SKILL.md](../skills/providers/SKILL.md)
