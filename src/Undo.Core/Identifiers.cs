namespace Undo.Core;

public readonly record struct EntityId
{
    public EntityId(string value)
    {
        Value = RequireValue(value, nameof(value));
    }

    public string Value { get; }

    public override string ToString() => Value;

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Stable IDs cannot be empty.", parameterName);
        }

        return value;
    }
}

public readonly record struct EventId
{
    public EventId(string value)
    {
        Value = RequireValue(value, nameof(value));
    }

    public string Value { get; }

    public override string ToString() => Value;

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Stable IDs cannot be empty.", parameterName);
        }

        return value;
    }
}

public readonly record struct LocationId
{
    public LocationId(string value)
    {
        Value = RequireValue(value, nameof(value));
    }

    public string Value { get; }

    public override string ToString() => Value;

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Stable IDs cannot be empty.", parameterName);
        }

        return value;
    }
}

public readonly record struct PuzzleId
{
    public PuzzleId(string value)
    {
        Value = RequireValue(value, nameof(value));
    }

    public string Value { get; }

    public override string ToString() => Value;

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Stable IDs cannot be empty.", parameterName);
        }

        return value;
    }
}
