using Undo.Core.Corrections;
using Undo.Core.Events;

namespace Undo.Core.State;

public sealed class EffectiveStateResolver
{
    public StateValue Resolve(
        StateKey key,
        WorldStateStore worldState,
        EventLedger ledger,
        CorrectionState corrections)
    {
        ArgumentNullException.ThrowIfNull(worldState);
        ArgumentNullException.ThrowIfNull(ledger);
        ArgumentNullException.ThrowIfNull(corrections);

        var effective = worldState.GetInitial(key);

        foreach (var recordedEvent in ledger.Events)
        {
            if (corrections.IsSuppressed(recordedEvent.Id))
            {
                continue;
            }

            foreach (var mutation in recordedEvent.Mutations)
            {
                if (mutation.Key == key)
                {
                    effective = mutation.ResultingValue;
                }
            }
        }

        return effective;
    }
}
