using Undo.Core.Events;

namespace Undo.Core.Corrections;

public sealed class CorrectionManager
{
    private readonly EventLedger _ledger;
    private readonly CorrectionState _state;

    public CorrectionManager(EventLedger ledger, CorrectionState state)
    {
        _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public void Suppress(EventId eventId)
    {
        if (!_ledger.TryGet(eventId, out var recordedEvent) || recordedEvent is null)
        {
            throw new KeyNotFoundException($"Event {eventId} does not exist in the active ledger.");
        }

        if (!recordedEvent.Reversible)
        {
            throw new InvalidOperationException($"Event {eventId} is not reversible.");
        }

        if (_state.IsSuppressed(eventId))
        {
            throw new InvalidOperationException($"Event {eventId} is already suppressed.");
        }

        if (_state.SuppressedEvents.Count >= _state.Capacity)
        {
            throw new InvalidOperationException("Correction capacity is full.");
        }

        _state.Add(eventId);
    }

    public void Release(EventId eventId)
    {
        _state.Remove(eventId);
    }
}
