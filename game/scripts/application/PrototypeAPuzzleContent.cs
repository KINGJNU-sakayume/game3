using System.Text.Json;

namespace Undo.Game.Application;

public sealed class PrototypeAPuzzleContent
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public int SchemaVersion { get; init; }

    public string PuzzleId { get; init; } = string.Empty;

    public string DisplayNameKey { get; init; } = string.Empty;

    public string ScenePath { get; init; } = string.Empty;

    public int CorrectionCapacity { get; init; }

    public List<string> Locations { get; init; } = [];

    public Dictionary<string, Dictionary<string, JsonElement>> InitialState { get; init; } =
        new(StringComparer.Ordinal);

    public List<PrototypeAActorContent> Actors { get; init; } = [];

    public static PrototypeAPuzzleContent Load(string resourcePath)
    {
        var json = Godot.FileAccess.GetFileAsString(resourcePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException($"Prototype content is empty or unreadable: {resourcePath}");
        }

        return JsonSerializer.Deserialize<PrototypeAPuzzleContent>(json, SerializerOptions)
            ?? throw new InvalidOperationException($"Prototype content could not be deserialized: {resourcePath}");
    }
}

public sealed class PrototypeAActorContent
{
    public string ActorId { get; init; } = string.Empty;

    public string InitialLocation { get; init; } = string.Empty;

    public Dictionary<string, JsonElement> InitialMemory { get; init; } =
        new(StringComparer.Ordinal);

    public List<PrototypeARoutineStepContent> Routine { get; init; } = [];
}

public sealed class PrototypeARoutineStepContent
{
    public string Type { get; init; } = string.Empty;

    public string? Location { get; init; }

    public double? DurationSeconds { get; init; }

    public string? Verb { get; init; }

    public string? Target { get; init; }

    public bool Reversible { get; init; }

    public string? Source { get; init; }
}
