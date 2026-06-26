// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.Abstractions.Responses;

namespace TransferX.Provider.MyProvider.Queries;

/// <summary>
/// Listet Dateien in einem Verzeichnis auf.<br/>
/// TODO: Provider-spezifische Logik implementieren.
/// </summary>
internal sealed class ListFilesQuery : IProviderQuery<ListFilesRequest, ListFilesResponse>
{
    private readonly string _basePath;
    private readonly ProviderCredentials? _credentials;

    /// <summary>Erstellt eine neue Instanz von <see cref="ListFilesQuery"/>.</summary>
    public ListFilesQuery(string basePath, ProviderCredentials? credentials)
    {
        this._basePath = basePath;
        this._credentials = credentials;
    }

    /// <inheritdoc/>
    public Task<ListFilesResponse> ExecuteAsync(
        ListFilesRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _ = this._basePath;
        _ = this._credentials;

        return Task.FromResult(new ListFilesResponse
        {
            Items = Array.Empty<FileItem>(),
            SearchedPath = request.Path,
            WasRecursive = request.Recursive
        });
    }
}
