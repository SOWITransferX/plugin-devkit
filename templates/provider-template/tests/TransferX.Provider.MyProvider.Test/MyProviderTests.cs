// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransferX.Provider.Abstractions.Models;
using TransferX.Provider.Abstractions.Requests;

namespace TransferX.Provider.MyProvider.Test;

/// <summary>
/// Basis-Tests fuer das MyProvider-Scaffold-Plugin.
/// </summary>
[TestClass]
public sealed class MyProviderTests
{
    /// <summary>
    /// Prueft Metadaten und parameterlosen Konstruktor fuer Plugin-Discovery.
    /// </summary>
    [TestMethod]
    public void MyProvider_HasMetadataAndParameterlessConstructor()
    {
        var provider = new MyProvider();

        Assert.AreEqual("MyProvider Provider", provider.Name);
        Assert.AreEqual("MyProviderType", provider.ProviderType);
        Assert.IsFalse(string.IsNullOrWhiteSpace(provider.Version));
    }

    /// <summary>
    /// ExecuteAsync ohne InitializeAsync wirft InvalidOperationException.
    /// </summary>
    [TestMethod]
    public async Task ExecuteAsync_WithoutInitialize_ThrowsInvalidOperationException()
    {
        var provider = new MyProvider();

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => provider.ExecuteAsync(
                new ListFilesRequest { Path = "/" }));
    }

    /// <summary>
    /// InitializeAsync mit leerem BasePath wirft ArgumentException.
    /// </summary>
    [TestMethod]
    public async Task InitializeAsync_EmptyBasePath_ThrowsArgumentException()
    {
        var provider = new MyProvider();
        var config = new ProviderConfigItem
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            ProviderType = "MyProviderType",
            BasePath = string.Empty
        };

        await Assert.ThrowsExceptionAsync<ArgumentException>(
            () => provider.InitializeAsync(config));
    }
}
