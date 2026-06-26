// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;

namespace TransferX.Provider.MyProvider.Commands;

/// <summary>
/// Laedt eine Datei zum Provider hoch.<br/>
/// TODO: Provider-spezifische Upload-Logik implementieren.
/// </summary>
internal sealed class UploadFileCommand : IProviderCommand<UploadFileRequest, UploadFileResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    /// <summary>Erstellt eine neue Instanz von <see cref="UploadFileCommand"/>.</summary>
    public UploadFileCommand(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }

    /// <inheritdoc/>
    public async Task<UploadFileResponse> ExecuteAsync(
        UploadFileRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = this._basePath;
        _ = this._credentials;
        await using var stream = await request.ContentFactory(cancellationToken);

        // TODO: Upload-Logik implementieren
        return new UploadFileResponse
        {
            Success = false,
            Path = request.TargetPath,
            BytesTransferred = 0
        };
    }
}
