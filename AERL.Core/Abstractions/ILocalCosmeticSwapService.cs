using AERL.Core.Models;

namespace AERL.Core.Abstractions;

public interface ILocalCosmeticSwapService
{
    IReadOnlyList<RocketLeagueItem> Items { get; }
    IReadOnlyList<LocalSwapState> ActiveSwaps { get; }
    bool IsReady { get; }
    string Status { get; }

    Task InitializeAsync(CancellationToken cancellationToken = default);
    IEnumerable<string> GetSlots();
    IEnumerable<RocketLeagueItem> Search(string slot, string? query, int max = 250);
    Task<LocalSwapState> ApplyAsync(RocketLeagueItem equippedItem, RocketLeagueItem visualItem, CancellationToken cancellationToken = default);
    Task<LocalSwapState> ApplyCustomFileAsync(string sourceFile, string targetPackageName, string displayName, CancellationToken cancellationToken = default);
    Task RestoreAsync(LocalSwapState state, CancellationToken cancellationToken = default);
    Task RestoreAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> VerifyAsync(CancellationToken cancellationToken = default);

    Task<string> ApplyKnownGoodAlphaBoostAsync(string bundledDirectory, CancellationToken cancellationToken = default);
    Task<string> ApplyKnownGoodAlphaWheelsAsync(string bundledDirectory, CancellationToken cancellationToken = default);
    Task<string> RestoreKnownGoodAlphaBoostAsync(CancellationToken cancellationToken = default);
    Task<string> RestoreKnownGoodAlphaWheelsAsync(CancellationToken cancellationToken = default);
}
