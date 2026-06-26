// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace TransferX.Transfer.MyCommand.Helpers;

/// <summary>
/// Hilfklasse fuer Pfad-Mapping zwischen Quelle und Ziel (optional).<br/>
/// TODO: Implementieren fuer Sync-/Mirror-Transfers.
/// </summary>
public static class PathMapper
{
    /// <summary>
    /// Mappt einen Quellpfad auf den Zielpfad.
    /// </summary>
    /// <param name="sourcePath">Quellpfad.</param>
    /// <param name="sourceRoot">Quell-Root.</param>
    /// <param name="targetRoot">Ziel-Root.</param>
    /// <returns>Zielpfad.</returns>
    public static string MapToTarget(string sourcePath, string sourceRoot, string targetRoot)
    {
        throw new NotImplementedException("PathMapper.MapToTarget ist noch nicht implementiert.");
    }
}
