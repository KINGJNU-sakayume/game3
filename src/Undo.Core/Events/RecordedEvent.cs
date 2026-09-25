using Undo.Core.State;

namespace Undo.Core.Events;

public sealed class RecordedEvent
{
    private readonly IReadOnlyList<StateMutation> _mutations;

    public RecordedEvent(
        EventId id,
        long sequence,
        DateTimeOffset timestamp,
        EntityId actorId,
        EventVerb verb,
        EntityId targetId,
        IEnumerable<StateMutation> mutations,
        bool reversible,
        RecordSource source)
    {
        if (sequence <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "Event sequence must be positive.");
        }

        ArgumentNullException.ThrowIfNull(mutations);

        var copiedMutations = mutations.ToArray();
        if (copiedMutations.Length == 0)
        {
            throw new ArgumentException("A recorded state-changing event must contain at least one mutation.", nameof(mutations));
        }

        Id = id;
        Sequence = sequence;
        Timestamp = timestamp;
        ActorId = actorId;
        Verb = verb;
        TargetId = targetId;
        _mutations = Array.AsReadOnly(copiedMutations);
        Reversible = reversible;
        Source = source;
    }

    public EventId Id { get; }

    public long Sequence { get; }

    public DateTimeOffset Timestamp { get; }

    public EntityId ActorId { get; }

    public EventVerb Verb { get; }

    public EntityId TargetId { get; }

    public IReadOnlyList<StateMutation> Mutations => _mutations;

    public bool Reversible { get; }

    public RecordSource Source { get; }
}
