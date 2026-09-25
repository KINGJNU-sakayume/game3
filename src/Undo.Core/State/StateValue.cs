using System.Globalization;

namespace Undo.Core.State;

public enum StateValueKind
{
    Boolean,
    Integer,
    Identifier,
}

public readonly record struct StateValue
{
    private StateValue(StateValueKind kind, string value)
    {
        Kind = kind;
        Value = value;
    }

    public StateValueKind Kind { get; }

    public string Value { get; }

    public static StateValue Boolean(bool value) =>
        new(StateValueKind.Boolean, value ? "true" : "false");

    public static StateValue Integer(long value) =>
        new(StateValueKind.Integer, value.ToString(CultureInfo.InvariantCulture));

    public static StateValue Identifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Identifier state values cannot be empty.", nameof(value));
        }

        return new StateValue(StateValueKind.Identifier, value);
    }

    public bool AsBoolean()
    {
        if (Kind != StateValueKind.Boolean)
        {
            throw new InvalidOperationException($"State value is {Kind}, not Boolean.");
        }

        return bool.Parse(Value);
    }

    public override string ToString() => Value;
}
