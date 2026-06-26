// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;

namespace TransferX.Provider.MyProvider.Queries;

/// <summary>
/// Laedt eine Datei vom Provider herunter.<br/>
/// TODO: Provider-spezifische Download-Logik implementieren.
/// </summary>
internal sealed class DownloadFileQuery : IProviderQuery<DownloadFileRequest, DownloadFileResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    /// <summary>Erstellt eine neue Instanz von <see cref="DownloadFileQuery"/>.</summary>
    public DownloadFileQuery(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }

    /// <inheritdoc/>
    public Task<DownloadFileResponse> ExecuteAsync(
        DownloadFileRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = this._basePath;
        _ = this._credentials;

        // TODO: Download-Logik implementieren
        throw new NotImplementedException("DownloadFileQuery ist noch nicht implementiert.");
    }
}
