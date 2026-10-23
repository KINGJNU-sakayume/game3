using Undo.Core.Events;
using Undo.Core.Puzzles;
using Undo.Core.State;
using Xunit;

namespace Undo.Core.Tests;

public sealed class PrototypeBGoldenTests
{
    private static readonly EntityId GuardId = new("GUARD_01");
    private static readonly EntityId BadgeId = new("BADGE_01");
    private static readonly EntityId ScannerId = new("SCANNER_01");
    private static readonly EntityId DoorId = new("STAFF_DOOR_A");
    private static readonly EventId TakeId = new("E1");
    private static readonly EventId AuthorizeId = new("E2");
    private static readonly EventId UnlockId = new("E3");

    private static readonly StateKey BadgePossession = new(BadgeId, StateChannel.Possession);
    private static readonly StateKey BadgePosition = new(BadgeId, StateChannel.Position);
    private static readonly StateKey ScannerAuthorization = new(ScannerId, StateChannel.Authorization);
    private static readonly StateKey DoorLock = new(DoorId, StateChannel.Lock);

    private const string DeskLocation = "LOC_PT_B_BADGE_DESK";
    private const string CarriedLocation = "LOC_CARRIED_BY_GUARD";

    [Fact]
    public void SuppressTakeAfterUnlock_BadgeReturns_DoorStaysUnlocked()
    {
        var session = CreateSession();
        RecordTake(session);
        RecordAuthorize(session);
        RecordUnlock(session);

        session.CorrectionManager.Suppress(TakeId);

        Assert.Equal("NONE", session.Resolve(BadgePossession).Value);
        Assert.Equal(DeskLocation, session.Resolve(BadgePosition).Value);
        Assert.Equal("GRANTED", session.Resolve(ScannerAuthorization).Value);
        Assert.Equal("UNLOCKED", session.Resolve(DoorLock).Value);
        Assert.Equal(3, session.EventLedger.Events.Count);
        Assert.Equal([TakeId, AuthorizeId, UnlockId], session.EventLedger.Events.Select(e => e.Id).ToArray());
        Assert.Equal(GuardId, session.EventLedger.Events[0].ActorId);
    }

    [Fact]
    public void SuppressTakeAfterAuthorize_AuthorizationPersists()
    {
        var session = CreateSession();
        RecordTake(session);
        RecordAuthorize(session);

        session.CorrectionManager.Suppress(TakeId);

        Assert.Equal("NONE", session.Resolve(BadgePossession).Value);
        Assert.Equal(DeskLocation, session.Resolve(BadgePosition).Value);
        Assert.Equal("GRANTED", session.Resolve(ScannerAuthorization).Value);
        Assert.Equal("LOCKED", session.Resolve(DoorLock).Value);
        Assert.Equal(2, session.EventLedger.Events.Count);
    }

    [Fact]
    public void ReleaseTake_DoesNotDeleteAuthorizeOrUnlock()
    {
        var session = CreateSession();
        RecordTake(session);
        RecordAuthorize(session);
        RecordUnlock(session);

        session.CorrectionManager.Suppress(TakeId);
        session.CorrectionManager.Release(TakeId);

        Assert.Equal(GuardId.Value, session.Resolve(BadgePossession).Value);
        Assert.Equal(CarriedLocation, session.Resolve(BadgePosition).Value);
        Assert.Equal("GRANTED", session.Resolve(ScannerAuthorization).Value);
        Assert.Equal("UNLOCKED", session.Resolve(DoorLock).Value);
        Assert.Equal(3, session.EventLedger.Events.Count);
        Assert.Empty(session.Corrections.SuppressedEvents);
    }

    [Fact]
    public void RestorePrototypeB_ReturnsInitialState()
    {
        var session = CreateSession();
        RecordTake(session);
        RecordAuthorize(session);
        RecordUnlock(session);
        session.CorrectionManager.Suppress(TakeId);

        session.RestoreCurrentRecord();

        Assert.Equal("NONE", session.Resolve(BadgePossession).Value);
        Assert.Equal(DeskLocation, session.Resolve(BadgePosition).Value);
        Assert.Equal("DENIED", session.Resolve(ScannerAuthorization).Value);
        Assert.Equal("LOCKED", session.Resolve(DoorLock).Value);
        Assert.Empty(session.EventLedger.Events);
        Assert.Empty(session.Corrections.SuppressedEvents);
    }

    [Fact]
    public void TakeAuthorizeUnlock_RecordsThreeDistinctOrderedEvents()
    {
        var session = CreateSession();
        var take = RecordTake(session);
        var authorize = RecordAuthorize(session);
        var unlock = RecordUnlock(session);

        Assert.Equal([1L, 2L, 3L], session.EventLedger.Events.Select(e => e.Sequence).ToArray());
        Assert.Equal([EventVerb.Take, EventVerb.Authorize, EventVerb.Unlock],
            session.EventLedger.Events.Select(e => e.Verb).ToArray());
        Assert.Same(take, session.EventLedger.Events[0]);
        Assert.Same(authorize, session.EventLedger.Events[1]);
        Assert.Same(unlock, session.EventLedger.Events[2]);
        Assert.Equal(BadgeId, take.TargetId);
        Assert.Equal(ScannerId, authorize.TargetId);
        Assert.Equal(DoorId, unlock.TargetId);
        Assert.Equal(2, take.Mutations.Count);
        Assert.Single(authorize.Mutations);
        Assert.Single(unlock.Mutations);
    }

    private static PuzzleSession CreateSession()
    {
        var world = new WorldStateStore();
        world.AddInitial(BadgePossession, StateValue.Identifier("NONE"));
        world.AddInitial(BadgePosition, StateValue.Identifier(DeskLocation));
        world.AddInitial(ScannerAuthorization, StateValue.Identifier("DENIED"));
        world.AddInitial(DoorLock, StateValue.Identifier("LOCKED"));
        return new PuzzleSession(world, correctionCapacity: 1);
    }

    private static RecordedEvent RecordTake(PuzzleSession session) =>
        session.EventRecorder.Record(
            TakeId, DateTimeOffset.UnixEpoch, GuardId, EventVerb.Take, BadgeId,
            [
                new StateChange(BadgePossession, StateValue.Identifier(GuardId.Value)),
                new StateChange(BadgePosition, StateValue.Identifier(CarriedLocation)),
            ],
            reversible: true, RecordSource.AccessControl);

    private static RecordedEvent RecordAuthorize(PuzzleSession session) =>
        session.EventRecorder.Record(
            AuthorizeId, DateTimeOffset.UnixEpoch.AddSeconds(1), GuardId,
            EventVerb.Authorize, ScannerId,
            [new StateChange(ScannerAuthorization, StateValue.Identifier("GRANTED"))],
            reversible: true, RecordSource.AccessControl);

    private static RecordedEvent RecordUnlock(PuzzleSession session) =>
        session.EventRecorder.Record(
            UnlockId, DateTimeOffset.UnixEpoch.AddSeconds(2), ScannerId,
            EventVerb.Unlock, DoorId,
            [new StateChange(DoorLock, StateValue.Identifier("UNLOCKED"))],
            reversible: true, RecordSource.AccessControl);
}
