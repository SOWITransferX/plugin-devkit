# GitHub Copilot — plugin-devkit

Lies zuerst [AGENTS.md](../AGENTS.md) und befolge alle dort genannten Anweisungen.

## Verweise

| Bereich | Pfad |
| --- | --- |
| Skills | [skills/](../skills/) |
| Dokumentation | [docs/](../docs/) |
| Templates | [templates/](../templates/) |
| Beispiele | [examples/](../examples/) |
| NuGet-Versionen | [sdk/package-versions.json](../sdk/package-versions.json) |

## Kurzregeln

- Datei-Header: SOWI Informatik, www.sowi.ch / Franz Schönbächler
- Kommentare und XML-Docs: Deutsch
- Codestil: [.editorconfig](../.editorconfig)
- NuGet-Versionen aus `sdk/package-versions.json`
- Provider: parameterloser Konstruktor für Plugin-Discovery
- Transfer-Plugins: keine `TransferX.Core`-Referenz
