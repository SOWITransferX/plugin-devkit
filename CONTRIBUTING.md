# Contributing — plugin-devkit

Beiträge unterliegen der [MIT-Lizenz](LICENSE.md) des Repositories.

## Guidelines

- Alle C#- und CSHTML-Dateien müssen den Datei-Header enthalten:

  ```
  SOWI Informatik, www.sowi.ch
  Franz Schönbächler
  ```

- Erklärungen in Chat und Code-Kommentaren (// und /// XML Comments) sind auf **Deutsch**.
- Formatierung und Codestil werden durch [.editorconfig](.editorconfig) vorgegeben.
- Bestehende Kommentare belassen, ausser sie sind durch Änderungen nicht mehr korrekt.
- Bei Änderungen an bestehendem Code: Begründung in Commit-Nachricht und ggf. in Kommentaren dokumentieren.

## Dokumentations-Sync

- **Owner der Wahrheit:** `Data\Repositories\TransferX\Source`
- **Sync-Methode:** Kopie (nie Submodule, Symlinks oder direkte Referenzen)
- **Auslöser:** bei Bedarf, Abstractions-Release oder manuell per KI-Agent
- Details: [docs/MIGRATION.md](docs/MIGRATION.md)

## Templates

- Neue Plugin-Scaffolds gehören in `templates/`, nicht als PowerShell-Generatoren.
- NuGet-Versionen in Templates verweisen auf [sdk/package-versions.json](sdk/package-versions.json).

## Referenz-Repositories

- Provider-Dummy: https://github.com/SOWITransferX/Provider-Dummy
- Transfer-Dummy: https://github.com/SOWITransferX/Transfer-Dummy

Referenz-Implementierungen werden **nicht** ins DevKit kopiert — nur verlinkt in `examples/`.
