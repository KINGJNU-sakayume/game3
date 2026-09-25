using Undo.Core.Corrections;
using Undo.Core.Events;
using Undo.Core.State;
using Xunit;

namespace Undo.Core.Tests;

public sealed class CorrectionGoldenTests
{
    [Fact]
    public void CloseSuppressRelease_ResolvesDoorStateWithoutRewritingHistory()
    {
        var doorId = new EntityId("DOOR_A");
        var workerId = new EntityId("NPC_WORKER_01");
        var closeEventId = new EventId("E1");
        var doorOpenKey = new StateKey(doorId, StateChannel.Open);

        var worldState = new WorldStateStore();
        worldState.AddInitial(doorOpenKey, StateValue.Boolean(true));

        var ledger = new EventLedger();
        var closeEvent = new RecordedEvent(
            closeEventId,
            sequence: 1,
            timestamp: DateTimeOffset.UnixEpoch,
            actorId: workerId,
            verb: EventVerb.Close,
            targetId: doorId,
            mutations:
            [
                new StateMutation(
                    doorOpenKey,
                    PreviousValue: StateValue.Boolean(true),
                    ResultingValue: StateValue.Boolean(false)),
            ],
            reversible: true,
            source: RecordSource.AccessControl);

        ledger.Append(closeEvent);

        var corrections = new CorrectionState(capacity: 1);
        var correctionManager = new CorrectionManager(ledger, corrections);
        var resolver = new EffectiveStateResolver();

        Assert.False(resolver.Resolve(doorOpenKey, worldState, ledger, corrections).AsBoolean());

        correctionManager.Suppress(closeEventId);

        Assert.True(resolver.Resolve(doorOpenKey, worldState, ledger, corrections).AsBoolean());
        Assert.Single(ledger.Events);
        Assert.Same(closeEvent, ledger.Events[0]);

        correctionManager.Release(closeEventId);

        Assert.False(resolver.Resolve(doorOpenKey, worldState, ledger, corrections).AsBoolean());
        Assert.Single(ledger.Events);
        Assert.Same(closeEvent, ledger.Events[0]);
    }
}
