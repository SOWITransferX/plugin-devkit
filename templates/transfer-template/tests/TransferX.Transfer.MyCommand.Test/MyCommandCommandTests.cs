// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TransferX.Provider.Abstractions;
using TransferX.Transfer.Abstractions;
using TransferX.Transfer.Abstractions.Progress;

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

        var result = await command.ExecuteAsync(CreateContext());

        Assert.IsTrue(result.Success);
        Assert.AreEqual(0, result.TotalFiles);
        Assert.AreEqual(0, result.FailedFiles);
    }

    /// <summary>
    /// Abgebrochenes CancellationToken wirft OperationCanceledException.
    /// </summary>
    [TestMethod]
    public async Task ExecuteAsync_Cancelled_ThrowsOperationCanceledException()
    {
        var command = new MyCommandCommand();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(
            () => command.ExecuteAsync(CreateContext(), cts.Token));
    }

    /// <summary>
    /// Referenztest: kein TransferX.Core.<br/>
    /// Die Assembly darf Core nicht binden. Die Build-Sperre der Abstractions-Pakete
    /// weist eine PackageReference zusaetzlich beim Build ab.
    /// </summary>
    [TestMethod]
    public void Referenztest_KeinTransferXCore()
    {
        var names = typeof(MyCommandCommand).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name);

        Assert.IsFalse(names.Contains("TransferX.Core"));
    }

    /// <summary>
    /// Baut einen Host-Kontext mit Mock-Providern und Mock fuer ITransferProgress.
    /// </summary>
    /// <returns>Ausfuehrungskontext fuer den Scaffold-Command.</returns>
    private static TransferContext CreateContext()
    {
        return new TransferContext
        {
            TransferId = Guid.NewGuid(),
            SourcePath = "/source",
            TargetPath = "/target",
            Source = new Mock<IProvider>().Object,
            Target = new Mock<IProvider>().Object,
            Progress = new Mock<ITransferProgress>().Object,
        };
    }
}
