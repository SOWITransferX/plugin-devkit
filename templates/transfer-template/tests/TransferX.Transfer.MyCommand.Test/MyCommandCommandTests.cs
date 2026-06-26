// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TransferX.Provider.Abstractions;
using TransferX.Transfer.Abstractions.Models;

namespace TransferX.Transfer.MyCommand.Test;

/// <summary>
/// Basis-Tests fuer das MyCommand-Scaffold-Plugin.
/// </summary>
[TestClass]
public sealed class MyCommandCommandTests
{
    /// <summary>
    /// Prueft Metadaten des Transfer-Commands.
    /// </summary>
    [TestMethod]
    public void MyCommandCommand_HasExpectedMetadata()
    {
        var command = new MyCommandCommand();

        Assert.AreEqual("MyCommand", command.CommandName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(command.Description));
        Assert.AreEqual("1.0.0", command.Version);
    }

    /// <summary>
    /// Stub-ExecuteAsync liefert leeres Erfolgsergebnis.
    /// </summary>
    [TestMethod]
    public async Task ExecuteAsync_ReturnsEmptySuccessResult()
    {
        var command = new MyCommandCommand();
        var sourceMock = new Mock<IProvider>();
        var targetMock = new Mock<IProvider>();
        var config = new TransferConfigItem
        {
            TransferId = Guid.NewGuid(),
            SourceProviderId = Guid.NewGuid(),
            SourcePath = "/source",
            TargetProviderId = Guid.NewGuid(),
            TargetPath = "/target",
            CommandName = "MyCommand"
        };

        var result = await command.ExecuteAsync(
            config,
            sourceMock.Object,
            targetMock.Object);

        Assert.IsTrue(result.Success);
        Assert.AreEqual(0, result.TotalFiles);
    }

    /// <summary>
    /// Abgebrochenes CancellationToken wirft OperationCanceledException.
    /// </summary>
    [TestMethod]
    public async Task ExecuteAsync_Cancelled_ThrowsOperationCanceledException()
    {
        var command = new MyCommandCommand();
        var sourceMock = new Mock<IProvider>();
        var targetMock = new Mock<IProvider>();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(
            () => command.ExecuteAsync(
                new TransferConfigItem
                {
                    TransferId = Guid.NewGuid(),
                    SourceProviderId = Guid.NewGuid(),
                    SourcePath = "/",
                    TargetProviderId = Guid.NewGuid(),
                    TargetPath = "/",
                    CommandName = "MyCommand"
                },
                sourceMock.Object,
                targetMock.Object,
                cancellationToken: cts.Token));
    }
}
