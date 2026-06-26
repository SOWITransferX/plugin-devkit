# plugin-devkit

Entwicklungs-Kit für **TransferX Provider-** und **Transfer-Plugins**.

Dieses Repository ist der zentrale Einstiegspunkt für menschliche Entwickler und KI-Coding-Agenten
(Cursor, GitHub Copilot, Claude und zukünftige Assistenten).

## Repository

- **GitHub:** https://github.com/SOWITransferX/plugin-devkit
- **Organisation:** SOWITransferX
- **Lokaler Pfad:** `Data\Repositories\TransferX\Plugins\plugin-devkit`

## Inhalt

| Bereich | Pfad | Beschreibung |
| --- | --- | --- |
| Agent-Einstieg | [AGENTS.md](AGENTS.md) | Master-Anleitung für alle KI-Agenten |
| Dokumentation | [docs/](docs/) | Architektur, Provider, Transfers, Konventionen |
| Skills | [skills/](skills/) | Spezialisierte Agent-Skills |
| Templates | [templates/](templates/) | Scaffold für neue Plugins |
| Beispiele | [examples/](examples/) | Links zu Referenz-Repositories |
| SDK | [sdk/](sdk/) | NuGet-Versionen und Vertragsdokumentation |

## Schnellstart

### Neues Provider-Plugin

1. [AGENTS.md](AGENTS.md) lesen
2. [templates/provider-template/](templates/provider-template/) als Basis verwenden
3. NuGet-Versionen aus [sdk/package-versions.json](sdk/package-versions.json) übernehmen
4. [docs/providers/implement-provider-plugin.md](docs/providers/implement-provider-plugin.md) befolgen

### Neues Transfer-Plugin

1. [AGENTS.md](AGENTS.md) lesen
2. [templates/transfer-template/](templates/transfer-template/) als Basis verwenden
3. NuGet-Versionen aus [sdk/package-versions.json](sdk/package-versions.json) übernehmen
4. [docs/transfers/implement-transfer-plugin.md](docs/transfers/implement-transfer-plugin.md) befolgen

## Ownership

| Repository | Verantwortlich für |
| --- | --- |
| **TransferX** (`Data\Repositories\TransferX\Source`) | Runtime, Core, Deployment, Abstractions-Quellcode |
| **plugin-devkit** (dieses Repo) | Plugin-Entwicklung, Skills, Templates, Developer-Dokumentation |

Dokumentation wird aus TransferX **kopiert** (nie verlinkt als Submodule). Details: [docs/MIGRATION.md](docs/MIGRATION.md).

## KI-Agent-Einstiegspunkte

| Assistent | Einstieg |
| --- | --- |
| Cursor | [AGENTS.md](AGENTS.md) + [.cursor/rules/plugin-devkit.mdc](.cursor/rules/plugin-devkit.mdc) |
| GitHub Copilot | [.github/copilot-instructions.md](.github/copilot-instructions.md) |
| Claude | [CLAUDE.md](CLAUDE.md) |
