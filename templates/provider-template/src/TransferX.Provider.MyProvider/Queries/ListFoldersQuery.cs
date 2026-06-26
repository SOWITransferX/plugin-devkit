// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;

namespace TransferX.Provider.MyProvider.Queries;

/// <summary>
/// Listet Ordner in einem Verzeichnis auf.<br/>
/// TODO: Provider-spezifische Logik implementieren.
/// </summary>
internal sealed class ListFoldersQuery : IProviderQuery<ListFoldersRequest, ListFoldersResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    /// <summary>Erstellt eine neue Instanz von <see cref="ListFoldersQuery"/>.</summary>
    public ListFoldersQuery(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }

    /// <inheritdoc/>
    public Task<ListFoldersResponse> ExecuteAsync(
        ListFoldersRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = this._basePath;
        _ = this._credentials;

        return Task.FromResult(new ListFoldersResponse
        {
            Items = Array.Empty<FolderItem>(),
            SearchedPath = request.Path,
            WasRecursive = request.Recursive
        });
    }
}
