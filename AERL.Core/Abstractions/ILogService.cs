using AERL.Core.Models;

namespace AERL.Core.Abstractions;

public interface ILogService
{
    event EventHandler<LogEntry>? EntryWritten;
    IReadOnlyList<LogEntry> Recent { get; }
    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Error(string message, Exception exception);
    void Debug(string message);
}
