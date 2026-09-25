using Undo.Core.Events;
using Undo.Core.Puzzles;
using Undo.Core.State;
using Xunit;

namespace Undo.Core.Tests;

public sealed class PrototypeATests
{
    private static readonly EntityId DoorId = new("DOOR_A");
    private static readonly EntityId WorkerId = new("NPC_WORKER_01");
    private static readonly EventId CloseEventId = new("E1");
    private static readonly StateKey DoorOpenKey = new(DoorId, StateChannel.Open);

    [Fact]
    public void InitialOpen_CloseEvent_EffectiveClosed()
    {
        var session = CreateSession();
        RecordClose(session);

        Assert.False(session.Resolve(DoorOpenKey).AsBoolean());
    }

    [Fact]
    public void SuppressClose_EffectiveOpen()
    {
        var session = CreateSession();
        RecordClose(session);

        session.CorrectionManager.Suppress(CloseEventId);

        Assert.True(session.Resolve(DoorOpenKey).AsBoolean());
    }

    [Fact]
    public void ReleaseClose_EffectiveClosed()
    {
        var session = CreateSession();
        RecordClose(session);
        session.CorrectionManager.Suppress(CloseEventId);

        session.CorrectionManager.Release(CloseEventId);

        Assert.False(session.Resolve(DoorOpenKey).AsBoolean());
    }

    [Fact]
    public void SuppressClose_DoesNotChangeNpcHistory()
    {
        var session = CreateSession();
        var closeEvent = RecordClose(session);

        session.CorrectionManager.Suppress(CloseEventId);

        Assert.Single(session.EventLedger.Events);
        Assert.Same(closeEvent, session.EventLedger.Events[0]);
        Assert.Equal(WorkerId, closeEvent.ActorId);
        Assert.Equal(EventVerb.Close, closeEvent.Verb);
        Assert.Equal(1, closeEvent.Sequence);
    }

    [Fact]
    public void Restore_ReturnsInitialState()
    {
        var session = CreateSession();
        RecordClose(session);
        session.CorrectionManager.Suppress(CloseEventId);

        session.RestoreCurrentRecord();

        Assert.True(session.Resolve(DoorOpenKey).AsBoolean());
        Assert.Empty(session.EventLedger.Events);
        Assert.Empty(session.Corrections.SuppressedEvents);
    }

    private static PuzzleSession CreateSession()
    {
        var worldState = new WorldStateStore();
        worldState.AddInitial(DoorOpenKey, StateValue.Boolean(true));
        worldState.AddInitial(
            new StateKey(DoorId, StateChannel.Lock),
            StateValue.Identifier("UNLOCKED"));

        return new PuzzleSession(worldState, correctionCapacity: 1);
    }

    private static RecordedEvent RecordClose(PuzzleSession session) =>
        session.EventRecorder.Record(
            CloseEventId,
            DateTimeOffset.UnixEpoch,
            WorkerId,
            EventVerb.Close,
            DoorId,
            [new StateChange(DoorOpenKey, StateValue.Boolean(false))],
            reversible: true,
            RecordSource.AccessControl);
}
