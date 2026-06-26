# Beispiele — Referenz-Repositories

Referenz-Implementierungen werden **nicht** ins DevKit kopiert.
Sie leben in eigenen GitHub-Repositories und dienen als vollstaendige, buildbare Referenz.

## Wann welches Artefakt?

| Ziel | Verwenden |
| --- | --- |
| Neues Plugin erstellen | [templates/](../templates/) |
| Fertige Patterns verstehen | GitHub-Referenz (unten) |
| Tests und Edge Cases | GitHub-Referenz klonen |

## Referenz-Repositories

| Plugin-Typ | Dokumentation | GitHub |
| --- | --- | --- |
| Provider | [provider-dummy.md](provider-dummy.md) | https://github.com/SOWITransferX/Provider-Dummy |
| Transfer | [transfer-dummy.md](transfer-dummy.md) | https://github.com/SOWITransferX/Transfer-Dummy |

## Lokale Owner-Pfade (Entwicklung)

| Repository | Lokaler Pfad |
| --- | --- |
| Provider-Dummy | `Data\Repositories\TransferX\Source\Plugins\Providers\Dummy` |
| Transfer-Dummy | `Data\Repositories\TransferX\Source\Plugins\Transfers\Dummy` |

Diese Pfade sind die Quelle der Wahrheit waehrend der lokalen Entwicklung.
Das DevKit verlinkt auf GitHub, damit KI-Agenten die Repos oeffentlich lesen koennen.
