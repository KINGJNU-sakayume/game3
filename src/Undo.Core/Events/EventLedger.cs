namespace Undo.Core.Events;

public sealed class EventLedger
{
    private readonly List<RecordedEvent> _events = new();

    public IReadOnlyList<RecordedEvent> Events => _events;

    public void Append(RecordedEvent recordedEvent)
    {
        ArgumentNullException.ThrowIfNull(recordedEvent);

        if (_events.Any(existing => existing.Id == recordedEvent.Id))
        {
            throw new InvalidOperationException($"Event ID {recordedEvent.Id} already exists.");
        }

        if (_events.Count > 0 && recordedEvent.Sequence <= _events[^1].Sequence)
        {
            throw new InvalidOperationException(
                $"Event sequence {recordedEvent.Sequence} must be greater than {_events[^1].Sequence}.");
        }

        _events.Add(recordedEvent);
    }

    public bool TryGet(EventId id, out RecordedEvent? recordedEvent)
    {
        recordedEvent = _events.FirstOrDefault(existing => existing.Id == id);
        return recordedEvent is not null;
    }
}
