using Undo.Core.Corrections;
using Undo.Core.State;

namespace Undo.Core.Events;

public sealed record StateChange(StateKey Key, StateValue ResultingValue);

public sealed class EventRecorder
{
    private readonly WorldStateStore _worldState;
    private readonly EventLedger _ledger;
    private readonly CorrectionState _corrections;
    private readonly EffectiveStateResolver _resolver;

    public EventRecorder(
        WorldStateStore worldState,
        EventLedger ledger,
        CorrectionState corrections,
        EffectiveStateResolver resolver)
    {
        _worldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
        _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
        _corrections = corrections ?? throw new ArgumentNullException(nameof(corrections));
        _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
    }

    public RecordedEvent Record(
        EventId eventId,
        DateTimeOffset timestamp,
        EntityId actorId,
        EventVerb verb,
        EntityId targetId,
        IEnumerable<StateChange> changes,
        bool reversible,
        RecordSource source)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var requestedChanges = changes.ToArray();
        if (requestedChanges.Length == 0)
        {
            throw new ArgumentException("A recorded event must contain at least one state change.", nameof(changes));
        }

        if (requestedChanges.Select(change => change.Key).Distinct().Count() != requestedChanges.Length)
        {
            throw new ArgumentException("A recorded event cannot mutate the same StateKey more than once.", nameof(changes));
        }

        var mutations = requestedChanges
            .Select(change => new StateMutation(
                change.Key,
                _resolver.Resolve(change.Key, _worldState, _ledger, _corrections),
                change.ResultingValue))
            .ToArray();

        var nextSequence = _ledger.Events.Count == 0
            ? 1
            : checked(_ledger.Events[^1].Sequence + 1);

        var recordedEvent = new RecordedEvent(
            eventId,
            nextSequence,
            timestamp,
            actorId,
            verb,
            targetId,
            mutations,
            reversible,
            source);

        _ledger.Append(recordedEvent);
        return recordedEvent;
    }
}
