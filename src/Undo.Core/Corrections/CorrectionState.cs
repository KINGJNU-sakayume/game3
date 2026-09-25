namespace Undo.Core.Corrections;

public sealed class CorrectionState
{
    private readonly List<EventId> _suppressedInOrder = new();
    private readonly HashSet<EventId> _suppressedLookup = new();

    public CorrectionState(int capacity)
    {
        if (capacity is < 1 or > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Correction capacity must be between 1 and 3.");
        }

        Capacity = capacity;
    }

    public int Capacity { get; }

    public IReadOnlyList<EventId> SuppressedEvents => _suppressedInOrder;

    public bool IsSuppressed(EventId eventId) => _suppressedLookup.Contains(eventId);

    internal void Add(EventId eventId)
    {
        if (!_suppressedLookup.Add(eventId))
        {
            throw new InvalidOperationException($"Event {eventId} is already suppressed.");
        }

        _suppressedInOrder.Add(eventId);
    }

    internal void Remove(EventId eventId)
    {
        if (!_suppressedLookup.Remove(eventId))
        {
            throw new InvalidOperationException($"Event {eventId} is not suppressed.");
        }

        _suppressedInOrder.Remove(eventId);
    }

    internal void Clear()
    {
        _suppressedInOrder.Clear();
        _suppressedLookup.Clear();
    }
}
