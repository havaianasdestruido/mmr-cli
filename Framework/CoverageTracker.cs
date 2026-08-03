namespace MmrCli.Framework;

public sealed class CoverageTracker
{
    private readonly Dictionary<string, (int Expected, int Present)> _data = new(StringComparer.OrdinalIgnoreCase);

    public void RegisterExpected(string dll, int expected)
    {
        if (!_data.ContainsKey(dll))
            _data[dll] = (expected, 0);
    }

    public void ReportPresent(string dll, int present)
    {
        if (_data.TryGetValue(dll, out var v))
            _data[dll] = (v.Expected, present);
    }

    public int TotalExpected => _data.Values.Sum(v => v.Expected);
    public int TotalPresent => _data.Values.Sum(v => v.Present);
    public IReadOnlyDictionary<string, (int Expected, int Present)> Data => _data;
}
