namespace Undo.Core.State;

public sealed record StateMutation(
    StateKey Key,
    StateValue PreviousValue,
    StateValue ResultingValue);
