using Undo.Core.Corrections;
using Undo.Core.Events;
using Undo.Core.State;

namespace Undo.Core.Puzzles;

public sealed class PuzzleSession
{
    public PuzzleSession(WorldStateStore worldState, int correctionCapacity)
    {
        WorldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
        EventLedger = new EventLedger();
        Corrections = new CorrectionState(correctionCapacity);
        Resolver = new EffectiveStateResolver();
        CorrectionManager = new CorrectionManager(EventLedger, Corrections);
        EventRecorder = new EventRecorder(WorldState, EventLedger, Corrections, Resolver);
    }

    public WorldStateStore WorldState { get; }

    public EventLedger EventLedger { get; }

    public CorrectionState Corrections { get; }

    public EffectiveStateResolver Resolver { get; }

    public CorrectionManager CorrectionManager { get; }

    public EventRecorder EventRecorder { get; }

    public StateValue Resolve(StateKey key) =>
        Resolver.Resolve(key, WorldState, EventLedger, Corrections);

    public void RestoreCurrentRecord()
    {
        Corrections.Clear();
        EventLedger.Clear();
    }
}
