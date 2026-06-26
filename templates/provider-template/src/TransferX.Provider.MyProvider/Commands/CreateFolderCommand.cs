// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;

namespace TransferX.Provider.MyProvider.Commands;

/// <summary>
/// Erstellt einen Ordner beim Provider.<br/>
/// TODO: Provider-spezifische Logik implementieren.
/// </summary>
internal sealed class CreateFolderCommand : IProviderCommand<CreateFolderRequest, CreateFolderResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    /// <summary>Erstellt eine neue Instanz von <see cref="CreateFolderCommand"/>.</summary>
    public CreateFolderCommand(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }

    /// <inheritdoc/>
    public Task<CreateFolderResponse> ExecuteAsync(
        CreateFolderRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = this._basePath;
        _ = this._credentials;

        // TODO: Ordner-Erstellung implementieren
        return Task.FromResult(new CreateFolderResponse
        {
            Success = false,
            Path = request.Path,
            WasCreated = false,
            ParentFoldersCreated = 0
        });
    }
}
