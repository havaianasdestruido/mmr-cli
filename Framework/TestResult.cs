namespace MmrCli.Framework;

public sealed class TestResult
{
    public required string Id { get; init; }
    public required string Dll { get; init; }
    public required string Group { get; init; }
    public required string Description { get; init; }
    public required TestStatus Status { get; set; }
    public required long DurationMs { get; set; }
    public string? Message { get; set; }
}
