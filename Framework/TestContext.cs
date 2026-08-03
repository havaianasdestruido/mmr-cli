namespace MmrCli.Framework;

public sealed class TestContext
{
    public required string BinDir { get; init; }
    public required CoverageTracker Coverage { get; init; }
    public required Native.NativeModuleCache Modules { get; init; }

    public IntPtr LoadModule(string dllFile) => Modules.Load(Path.Combine(BinDir, dllFile));
}
