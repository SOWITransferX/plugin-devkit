# SDK — NuGet-Pakete für Plugin-Entwicklung

<!-- Migriert und angepasst aus TransferX\Source\TransferX\docs\TransferXNuGetPackages.md, Stand: 2026-06-26 -->

Plugin-Projekte konsumieren die TransferX-Schnittstellen als **NuGet-Pakete** — kein Quellcode-Spiegel im DevKit.

## Kanonische Versionen

Alle Versionen stehen zentral in [package-versions.json](package-versions.json).

| NuGet-Paket-ID | Zweck | Typische Referenz in |
| --- | --- | --- |
| `TransferX.Provider.Abstractions` | Provider-Schnittstellen (`IProvider`, Commands, Queries) | Provider-Plugins |
| `TransferX.Transfer.Abstractions` | Transfer-Schnittstellen (`ITransferCommand`) | Transfer-Plugins |
| `TransferX.Domain` | Domain-Modelle, Value Objects (transitiv über Abstractions) | Selten direkt |

> **Wichtig:** Paketnamen sind **Plural** — `TransferX.Provider.Abstractions`, nicht `Abstraction`.

## PackageReference in `.csproj`

### Provider-Plugin

```xml
<ItemGroup>
  <PackageReference Include="TransferX.Provider.Abstractions" Version="26.5.5" />
</ItemGroup>
```

Version aus [package-versions.json](package-versions.json) übernehmen.

### Transfer-Plugin

```xml
<ItemGroup>
  <PackageReference Include="TransferX.Transfer.Abstractions" Version="26.5.5" />
</ItemGroup>
```

**Keine** `TransferX.Core`-Referenz in Transfer-Plugins.

## Voraussetzungen

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Zugriff auf den NuGet-Feed (nuget.org oder privater Feed der Organisation)

```powershell
dotnet --version
# Erwartet: 8.x.x
```

## Lokaler NuGet-Feed (Entwicklung)

Falls Pakete lokal aus dem TransferX-Repository gebaut werden:

```powershell
# Pakete bauen (im TransferX Source Repository)
dotnet pack Source\TransferX\src\TransferX.Provider.Abstractions\TransferX.Provider.Abstractions.csproj -c Release
dotnet pack Source\TransferX\src\TransferX.Transfer.Abstractions\TransferX.Transfer.Abstractions.csproj -c Release

# Lokale Quelle hinzufügen
dotnet nuget add source "C:\Data\Repositories\TransferX\Packages" --name TransferXLocal
```

## Vertragsdokumentation

Markdown-Referenz der Schnittstellen (kein C#-Quellcode):

- [contracts/provider-contracts.md](contracts/provider-contracts.md)
- [contracts/transfer-contracts.md](contracts/transfer-contracts.md)

## Paket-Erstellung (TransferX-Repository)

Die Erstellung und Veröffentlichung der NuGet-Pakete ist Aufgabe des **TransferX-Repositorys**, nicht des DevKits.
Owner: `Data\Repositories\TransferX\Source`.

## Weiterführend

- [docs/architecture/abstractions.md](../docs/architecture/abstractions.md)
- [docs/providers/implement-provider-plugin.md](../docs/providers/implement-provider-plugin.md)
- [docs/transfers/implement-transfer-plugin.md](../docs/transfers/implement-transfer-plugin.md)
