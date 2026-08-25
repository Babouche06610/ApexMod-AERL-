using AERL.Core.Models;

namespace AERL.Core.Abstractions;

public interface IGameDetector
{
    Task<GameSnapshot> DetectAsync(CancellationToken cancellationToken = default);
}
