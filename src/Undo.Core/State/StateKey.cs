namespace Undo.Core.State;

public readonly record struct StateKey(EntityId EntityId, StateChannel Channel);
