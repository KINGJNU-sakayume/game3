namespace Undo.Core.State;

public sealed class WorldStateStore
{
    private readonly Dictionary<StateKey, StateValue> _initialStates = new();

    public IReadOnlyDictionary<StateKey, StateValue> InitialStates => _initialStates;

    public void AddInitial(StateKey key, StateValue value)
    {
        if (!_initialStates.TryAdd(key, value))
        {
            throw new InvalidOperationException($"Initial state already exists for {key.EntityId}.{key.Channel}.");
        }
    }

    public StateValue GetInitial(StateKey key)
    {
        if (!_initialStates.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"No initial state exists for {key.EntityId}.{key.Channel}.");
        }

        return value;
    }
}
