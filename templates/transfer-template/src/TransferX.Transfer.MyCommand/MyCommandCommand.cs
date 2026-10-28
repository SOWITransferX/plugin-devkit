// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Transfer.Abstractions;
using TransferX.Transfer.Abstractions.Metadata;
using TransferX.Transfer.Abstractions.Models;

namespace TransferX.Transfer.MyCommand;

/// <summary>
/// MyCommand — Scaffold fuer ein TransferX Transfer-Plugin.<br/>
/// TODO: Transfer-Logik in ExecuteAsync implementieren.
/// </summary>
[TransferMetadata]
public sealed class MyCommandCommand : ITransferCommand
{
    /// <inheritdoc/>
    public string CommandName => "MyCommand";

    /// <inheritdoc/>
    public string Description =>
        "Scaffold-Transfer-Plugin ohne Dateiuebertragung; liefert ein leeres Erfolgsergebnis.";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <inheritdoc/>
    public Task<TransferResult> ExecuteAsync(
        TransferContext context,
        CancellationToken cancellationToken = default)
    {
        // SDK 3.0.0-preview.1 liefert Provider und Fortschritt über TransferContext.
        // Success, Dateizaehler und Bytes berechnet TransferResult; Duration entfaellt.
        _ = context;

        var fileResults = new List<FileTransferResult>();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // TODO: ListFiles → Download → Upload implementieren.
            // Fortschritt über context.Progress (ITransferProgress) melden.
            return Task.FromResult(new TransferResult
            {
                TotalFiles = 0,
                FileResults = fileResults.AsReadOnly()
            });
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return Task.FromResult(new TransferResult
            {
                TotalFiles = 0,
                FileResults = fileResults.AsReadOnly(),
                ErrorMessage = ex.Message
            });
        }
    }
}
