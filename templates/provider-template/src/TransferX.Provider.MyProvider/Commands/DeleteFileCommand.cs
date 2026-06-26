// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;

namespace TransferX.Provider.MyProvider.Commands;

/// <summary>
/// Loescht eine Datei beim Provider.<br/>
/// TODO: Provider-spezifische Logik implementieren.
/// </summary>
internal sealed class DeleteFileCommand : IProviderCommand<DeleteFileRequest, DeleteFileResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    /// <summary>Erstellt eine neue Instanz von <see cref="DeleteFileCommand"/>.</summary>
    public DeleteFileCommand(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }

    /// <inheritdoc/>
    public Task<DeleteFileResponse> ExecuteAsync(
        DeleteFileRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = this._basePath;
        _ = this._credentials;

        // TODO: Datei-Loeschung implementieren
        return Task.FromResult(new DeleteFileResponse
        {
            Success = false,
            Path = request.Path,
            WasDeleted = false,
            WasNotFound = true
        });
    }
}
