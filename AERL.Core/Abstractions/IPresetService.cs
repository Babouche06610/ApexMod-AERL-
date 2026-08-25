using AERL.Core.Models;

namespace AERL.Core.Abstractions;

public interface IPresetService
{
    IReadOnlyList<GaragePreset> Presets { get; }
    Task LoadAsync(CancellationToken cancellationToken = default);
    Task<GaragePreset> SaveAsync(GaragePreset preset, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
