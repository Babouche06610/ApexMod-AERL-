using System.Text.Json;
using System.Buffers.Binary;
using System.Reflection;
using System.Security.Cryptography;
using AERL.Core.Abstractions;
using AERL.Core.Models;
using AERL.Core.Services;

namespace AERL.SmokeTests;

internal static class Program
{
    private static async Task<int> Main()
    {
        var temp = Path.Combine(Path.GetTempPath(), "AERL-Smoke-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temp);

        try
        {
            await TestPresetsAsync(temp);
            await TestSessionHistoryAsync(temp);
            await TestPluginsAsync(temp);
            await TestMockStatsApiAsync(temp);
            TestBundledAesProvider();
            TestSeekFreeRelaxedKeyProbe();
            TestAlphaNameCollisionFreeing();
            Console.WriteLine("AERL_SMOKE_TESTS_OK");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("AERL_SMOKE_TESTS_FAILED");
            Console.Error.WriteLine(ex);
            return 1;
        }
        finally
        {
            try { Directory.Delete(temp, true); } catch { }
        }
    }

    private static async Task TestPresetsAsync(string temp)
    {
        var presets = new PresetService(temp);
        await presets.LoadAsync();
        Require(presets.Presets.Count == 1, "Default Garage preset should be created.");
        await presets.SaveAsync(new GaragePreset { Name = "Smoke preset", Body = "Fennec", Wheels = "OEM" });

        var reloaded = new PresetService(temp);
        await reloaded.LoadAsync();
        Require(reloaded.Presets.Any(x => x.Name == "Smoke preset"), "Garage preset should survive reload.");
    }

    private static async Task TestSessionHistoryAsync(string temp)
    {
        var history = new SessionHistoryService(temp);
        await history.LoadAsync();
        await history.AddAsync(new SessionRecord { Arena = "DFH Stadium", Winner = "Blue", PlaylistId = 13 });
        Require(history.Sessions.Count == 1, "Session history should save a record.");
    }

    private static async Task TestPluginsAsync(string temp)
    {
        var service = new PluginCatalogService(temp);
        var folder = Path.Combine(service.PluginDirectory, "smoke-plugin");
        Directory.CreateDirectory(folder);
        var manifest = new PluginManifest
        {
            Id = "smoke.plugin",
            Name = "Smoke Plugin",
            Permissions = ["HUD_WIDGET", "LOCAL_STORAGE"]
        };
        await File.WriteAllTextAsync(Path.Combine(folder, "manifest.json"), JsonSerializer.Serialize(manifest));
        await service.ScanAsync();
        Require(service.Plugins.Count == 1, "Plugin catalog should discover a manifest.");
        await service.SetEnabledAsync("smoke.plugin", false);
        await service.ScanAsync();
        Require(service.Plugins.Single().Enabled == false, "Plugin enabled state should persist.");
    }

    private static async Task TestMockStatsApiAsync(string temp)
    {
        var settings = new TestSettings(temp);
        settings.Current.MockMode = true;
        settings.Current.StatsApiEnabled = true;
        var logs = new TestLogs();
        await using var api = new RocketLeagueStatsApiService(settings, logs);
        await api.StartAsync();
        await Task.Delay(900);
        Require(api.IsConnected, "Mock Stats API should report connected.");
        Require(api.Current.Players.Count >= 3, "Mock Stats API should publish players.");
        await api.SendCommandAsync("LoadReplay", new { FileName = "smoke.replay" });
        Require(api.Current.IsReplay, "Mock LoadReplay command should enter replay mode.");
    }


    private static void TestBundledAesProvider()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "RuntimeData", "keys.txt");
        Require(File.Exists(path), "Bundled AES provider must be present in the release payload.");
        var count = File.ReadLines(path).Count(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#"));
        Require(count >= 1000, $"Bundled AES provider is truncated: {count} keys.");
    }

    private static void TestSeekFreeRelaxedKeyProbe()
    {
        var key = Enumerable.Range(1, 32).Select(i => (byte)i).ToArray();
        var plain = new byte[32];
        BinaryPrimitives.WriteInt32LittleEndian(plain.AsSpan(0, 4), 7); // sane chunk count
        BinaryPrimitives.WriteInt64LittleEndian(plain.AsSpan(4, 8), 0x77777777); // intentionally NOT depends_offset
        byte[] encrypted;
        using (var aes = Aes.Create())
        {
            aes.Key = key; aes.Mode = CipherMode.ECB; aes.Padding = PaddingMode.None;
            using var enc = aes.CreateEncryptor();
            encrypted = enc.TransformFinalBlock(plain, 0, plain.Length);
        }
        var method = typeof(UpkHeaderSwapEngine).GetMethod("MappedKeyLooksValid", BindingFlags.NonPublic | BindingFlags.Static);
        Require(method is not null, "SeekFree relaxed-key validator must exist.");
        // Build the private UpkPrefix record via ParsePrefix is overkill; invoke with a synthetic instance through reflection.
        var prefixType = typeof(UpkHeaderSwapEngine).GetNestedType("UpkPrefix", BindingFlags.NonPublic);
        Require(prefixType is not null, "UPK prefix type must exist.");
        var ctor = prefixType!.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Single();
        // record fields: totalHeader, nameCount, nameOffset, exportOffset, importOffset, dependsOffset, garbage, chunksOffset, metaOffset
        var prefix = ctor.Invoke(new object[] { 256, 4, 0, 0, 0, 1234, 0, 0, 0 });
        var ok = (bool)(method!.Invoke(null, new object[] { key, encrypted, prefix }) ?? false);
        Require(ok, "SeekFree exact-map key must pass relaxed validation even when unc_off != depends_offset.");
    }


    private static void TestAlphaNameCollisionFreeing()
    {
        static byte[] Entry(string name)
        {
            var raw = System.Text.Encoding.UTF8.GetBytes(name);
            var result = new byte[4 + raw.Length + 1 + 8];
            BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(0, 4), raw.Length + 1);
            Buffer.BlockCopy(raw, 0, result, 4, raw.Length);
            return result;
        }

        var header = Entry("Boost_AlphaReward").Concat(Entry("Boost_Standard")).ToArray();
        var method = typeof(UpkHeaderSwapEngine).GetMethod("ApplyInPlace", BindingFlags.NonPublic | BindingFlags.Static);
        Require(method is not null, "Collision-safe FName patcher must exist.");
        var pairs = new List<(string Old, string New)> { ("Boost_AlphaReward", "Boost_Standard") };
        var patched = (byte[])(method!.Invoke(null, new object[] { header, 2, pairs }) ?? throw new InvalidOperationException());
        var text = System.Text.Encoding.UTF8.GetString(patched);
        Require(text.Contains("Boost_Standard"), "Alpha root must be renamed to the target name.");
        Require(text.Contains("FREED"), "Pre-existing target FName must be freed instead of colliding.");
        Require(!text.Contains("Boost_AlphaReward"), "Donor Alpha root must not remain after collision-safe rename.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class TestSettings(string directory) : ISettingsService
    {
        public string DataDirectory { get; } = directory;
        public AppSettings Current { get; } = new();
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task SaveAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class TestLogs : ILogService
    {
        public event EventHandler<LogEntry>? EntryWritten;
        public IReadOnlyList<LogEntry> Recent => _entries;
        private readonly List<LogEntry> _entries = [];
        public void Info(string message) => Add(LogLevel.Info, message);
        public void Warn(string message) => Add(LogLevel.Warning, message);
        public void Error(string message) => Add(LogLevel.Error, message);
        public void Error(string message, Exception exception) => Add(LogLevel.Error, $"{message}: {exception.Message}{Environment.NewLine}{exception}");
        public void Debug(string message) => Add(LogLevel.Debug, message);
        private void Add(LogLevel level, string message)
        {
            var entry = new LogEntry(DateTime.Now, level, message);
            _entries.Add(entry);
            EntryWritten?.Invoke(this, entry);
        }
    }
}
