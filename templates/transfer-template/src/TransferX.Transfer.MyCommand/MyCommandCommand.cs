// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
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
        TransferConfigItem config,
        IProvider sourceProvider,
        IProvider targetProvider,
        IProgress<ProgressReport>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = config;
        _ = sourceProvider;
        _ = targetProvider;
        _ = progress;

        var startTime = DateTime.UtcNow;
        var fileResults = new List<FileTransferResult>();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // TODO: ListFiles → Download → Upload implementieren
            return Task.FromResult(new TransferResult
            {
                Success = true,
                TotalFiles = 0,
                SuccessfulFiles = 0,
                FailedFiles = 0,
                TotalBytesTransferred = 0,
                Duration = DateTime.UtcNow - startTime,
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
                Success = false,
                TotalFiles = 0,
                SuccessfulFiles = 0,
                FailedFiles = 0,
                TotalBytesTransferred = 0,
                Duration = DateTime.UtcNow - startTime,
                FileResults = fileResults.AsReadOnly(),
                ErrorMessage = ex.Message
            });
        }
    }
}
