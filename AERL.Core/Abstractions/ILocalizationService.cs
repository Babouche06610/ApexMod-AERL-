namespace AERL.Core.Abstractions;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    event EventHandler? LanguageChanged;
    string this[string key] { get; }
    Task InitializeAsync(string language, CancellationToken cancellationToken = default);
    Task SetLanguageAsync(string language, CancellationToken cancellationToken = default);
}
