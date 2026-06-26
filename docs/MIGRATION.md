# Dokumentations-Migration und Sync

## Owner der Wahrheit

| Inhalt | Owner-Pfad |
| --- | --- |
| TransferX Core, Abstractions-Quellcode, Runtime-Docs | `Data\Repositories\TransferX\Source` |
| plugin-devkit Docs, Skills, Templates | `Data\Repositories\TransferX\Plugins\plugin-devkit` |

## Sync-Regeln

### Regel 1: Copy, Never Link

Erlaubt: **COPY**, **DUPLICATE**, **GENERATE**, **EXPORT**

Verboten: Symlinks, Hard Links, Git Submodules, Git Subtrees, direkte Ordner-Referenzen

### Regel 2: Independent Evolution

DevKit-Dokumentation und TransferX-Dokumentation duerfen unabhaengig weiterentwickelt werden.

### Regel 3: Plugin Developer Focus

Nur plugin-relevante Dokumentation kopieren. Nicht kopieren: Deployment, Runtime-Internals, Operations, Infrastructure.

## Sync-Methode

**Ausloeser:** bei Bedarf, Abstractions-Release oder manuell (auch per KI-Agent)

**Schritte:**

1. Quelldatei in `TransferX\Source\TransferX\docs` identifizieren
2. Physische Kopie ins DevKit-Zielverzeichnis
3. Relative Links anpassen (siehe Mapping unten)
4. Provenance-Kommentar setzen: `<!-- Migriert aus TransferX\Source\TransferX\docs, Stand: YYYY-MM-DD -->`
5. `sdk/package-versions.json` bei NuGet-Aenderungen aktualisieren

## Mapping-Tabelle

| Quelle | DevKit-Ziel | Aktion |
| --- | --- | --- |
| `TransferXImplementProviderPlugin.md` | `docs/providers/implement-provider-plugin.md` | Kopieren |
| `TransferXImplementTransferPlugin.md` | `docs/transfers/implement-transfer-plugin.md` | Kopieren |
| `TransferXAbstractions.md` | `docs/architecture/abstractions.md` | Kopieren |
| `TransferXArchitektur.md` | `docs/architecture/architecture.md` | Kopieren (Plugin-Hinweis) |
| `CleanArchitectureCodingStandardsBestPractices.md` | `docs/conventions/coding-standards.md` | Kopieren |
| `TransferXNuGetPackages.md` | `sdk/README.md` | Teilweise (Konsumenten-Sicht) |
| `TransferXCore.md`, Api, Infrastructure, … | — | Nicht kopieren |
| `docfx/_site/` | — | Nicht kopieren |

## Link-Ersetzungen (bei Sync)

| Alt (TransferX) | Neu (DevKit) |
| --- | --- |
| `../ReadMe.md` | `../../README.md` |
| `TransferXArchitektur.md` | `../architecture/architecture.md` |
| `TransferXAbstractions.md` | `../architecture/abstractions.md` |
| `CleanArchitectureCodingStandardsBestPractices.md` | `../conventions/coding-standards.md` |
| `TransferXCore.md` | Nicht verlinken — Hinweis auf README |

## Referenz-Repositories (kein Copy)

Dummy-Implementierungen werden **nicht** ins DevKit kopiert:

| Referenz | GitHub |
| --- | --- |
| Provider-Dummy | https://github.com/SOWITransferX/Provider-Dummy |
| Transfer-Dummy | https://github.com/SOWITransferX/Transfer-Dummy |

Verweise in [examples/](../examples/).
