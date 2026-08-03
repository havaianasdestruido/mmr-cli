using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using MmrCli.Framework;

namespace MmrCli.Reporting;

public static class Reporters
{
    public static void WriteJUnit(string path, IReadOnlyList<TestResult> results)
    {
        var settings = new XmlWriterSettings { Indent = true, Encoding = new System.Text.UTF8Encoding(false) };
        int failures = results.Count(r => r.Status == TestStatus.Failed);
        int skipped = results.Count(r => r.Status == TestStatus.Skipped);

        using var writer = XmlWriter.Create(path, settings);
        writer.WriteStartDocument();
        writer.WriteStartElement("testsuites");
        writer.WriteAttributeString("tests", results.Count.ToString());
        writer.WriteAttributeString("failures", failures.ToString());
        writer.WriteAttributeString("skipped", skipped.ToString());

        writer.WriteStartElement("testsuite");
        writer.WriteAttributeString("name", "mmr-cli");
        writer.WriteAttributeString("tests", results.Count.ToString());
        writer.WriteAttributeString("failures", failures.ToString());
        writer.WriteAttributeString("skipped", skipped.ToString());

        foreach (var r in results)
        {
            writer.WriteStartElement("testcase");
            writer.WriteAttributeString("classname", r.Dll);
            writer.WriteAttributeString("name", r.Id);
            writer.WriteAttributeString("time", (r.DurationMs / 1000.0).ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));

            if (r.Status == TestStatus.Failed)
            {
                writer.WriteStartElement("failure");
                writer.WriteAttributeString("message", r.Message ?? "assertion failed");
                writer.WriteString(r.Message ?? "assertion failed");
                writer.WriteEndElement();
            }
            else if (r.Status == TestStatus.Skipped)
            {
                writer.WriteStartElement("skipped");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
    }

    public static void WriteJson(string path, IReadOnlyList<TestResult> results)
    {
        var payload = results.Select(r => new JsonCase
        {
            Id = r.Id,
            Dll = r.Dll,
            Group = r.Group,
            Description = r.Description,
            Status = r.Status.ToString(),
            DurationMs = r.DurationMs,
            Message = r.Message,
        });
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
        });
        File.WriteAllText(path, json);
    }

    private sealed class JsonCase
    {
        public string Id { get; set; } = "";
        public string Dll { get; set; } = "";
        public string Group { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public long DurationMs { get; set; }
        public string? Message { get; set; }
    }
}
