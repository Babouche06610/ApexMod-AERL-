using AERL.Core.Models;

namespace AERL.Core.Abstractions;

public interface IPluginCatalogService
{
    string PluginDirectory { get; }
    IReadOnlyList<PluginManifest> Plugins { get; }
    Task ScanAsync(CancellationToken cancellationToken = default);
    Task SetEnabledAsync(string id, bool enabled, CancellationToken cancellationToken = default);
}
