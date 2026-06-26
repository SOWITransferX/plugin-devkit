// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using TransferX.Domain.ValueObjects.Progress;
using TransferX.Provider.Abstractions;
using TransferX.Provider.Abstractions.Contracts;
using TransferX.Provider.Abstractions.Metadata;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;
using TransferX.Provider.MyProvider.Commands;
using TransferX.Provider.MyProvider.Queries;

namespace TransferX.Provider.MyProvider;

/// <summary>
/// MyProvider — Scaffold fuer ein TransferX Provider-Plugin.<br/>
/// TODO: Beschreibung und unterstuetzte Operationen ergaenzen.
/// </summary>
[ProviderMetadata]
public sealed class MyProvider : IProvider
{
    private string _basePath = string.Empty;
    private ProviderCredentials? _credentials;
    private bool _isInitialized;

    /// <summary>
    /// Erstellt eine Provider-Instanz fuer die Plugin-Discovery via <c>Activator.CreateInstance</c>.
    /// </summary>
    public MyProvider()
    {
    }

    /// <inheritdoc/>
    public string Name => "MyProvider Provider";

    /// <inheritdoc/>
    public string Version =>
        GetType().Assembly
            .GetName()
            .Version?
            .ToString(3) ?? "1.0.0";

    /// <inheritdoc/>
    public string ProviderType => "MyProviderType";

    /// <inheritdoc/>
    public Task InitializeAsync(ProviderConfigItem config, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(config);
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(config.BasePath))
        {
            throw new ArgumentException("BasePath darf nicht leer sein.", nameof(config));
        }

        // TODO: Provider-spezifische Validierung (Verbindung, Credentials)
        this._basePath = config.BasePath;
        this._credentials = config.Credentials;
        this._isInitialized = true;

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<ProviderResponse> ExecuteAsync(
        ProviderRequest request,
        IProgress<FileProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        this.EnsureInitialized();

        return request switch
        {
            ListFoldersRequest listFolders =>
                await new ListFoldersQuery(this._basePath, this._credentials)
                    .ExecuteAsync(listFolders, progress, cancellationToken),

            ListFilesRequest listFiles =>
                await new ListFilesQuery(this._basePath, this._credentials)
                    .ExecuteAsync(listFiles, progress, cancellationToken),

            DownloadFileRequest download =>
                await new DownloadFileQuery(this._basePath, this._credentials)
                    .ExecuteAsync(download, progress, cancellationToken),

            UploadFileRequest upload =>
                await new UploadFileCommand(this._basePath, this._credentials)
                    .ExecuteAsync(upload, progress, cancellationToken),

            CreateFolderRequest createFolder =>
                await new CreateFolderCommand(this._basePath, this._credentials)
                    .ExecuteAsync(createFolder, progress, cancellationToken),

            DeleteFileRequest deleteFile =>
                await new DeleteFileCommand(this._basePath, this._credentials)
                    .ExecuteAsync(deleteFile, progress, cancellationToken),

            _ => throw new NotSupportedException(
                $"Request-Typ '{request.GetType().Name}' wird von {this.ProviderType} nicht unterstuetzt.")
        };
    }

    private void EnsureInitialized()
    {
        if (!this._isInitialized)
        {
            throw new InvalidOperationException(
                "Provider wurde nicht initialisiert. InitializeAsync muss vor ExecuteAsync aufgerufen werden.");
        }
    }
}
