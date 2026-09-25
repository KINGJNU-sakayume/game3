using Godot;
using Undo.Game.Application;

namespace Undo.Game.Debug;

public partial class PrototypeAIntegration : Node
{
    public override async void _Ready()
    {
        var level = GetNode<PrototypeALevel>("Level");

        try
        {
            await ToSignal(
                GetTree().CreateTimer(6.5, processAlways: true),
                SceneTreeTimer.SignalName.Timeout);

            Require(level.RecordedEventCount == 1, "Expected exactly one CLOSE RecordedEvent.");
            Require(!level.DoorEffectiveOpen, "Door should be effectively CLOSED after historical CLOSE.");
            Require(
                level.WorkerSemanticLocation == "LOC_PT_A_EXIT",
                $"Worker should remain in present at LOC_PT_A_EXIT, got '{level.WorkerSemanticLocation}'.");

            var workerLocationAfterHistory = level.WorkerSemanticLocation;

            Require(level.CommitCloseCorrection(), "Correction commit should succeed.");
            Require(level.DoorEffectiveOpen, "Suppressing CLOSE should resolve DOOR_A.OPEN=true.");
            Require(level.RecordedEventCount == 1, "Suppression must not delete historical event.");
            Require(
                level.WorkerSemanticLocation == workerLocationAfterHistory,
                "Suppressing CLOSE must not rewind worker position/history.");

            Require(level.ReleaseCloseCorrection(), "Correction release should succeed.");
            Require(!level.DoorEffectiveOpen, "Release should resolve DOOR_A.OPEN=false.");
            Require(level.RecordedEventCount == 1, "Release must not create or replay historical CLOSE.");
            Require(
                level.WorkerSemanticLocation == workerLocationAfterHistory,
                "Release must not replay NPC close action.");

            level.RestoreCurrentRecord();

            Require(level.DoorEffectiveOpen, "Restore should return DOOR_A.OPEN=true.");
            Require(level.RecordedEventCount == 0, "Restore should clear runtime event ledger.");
            Require(!level.CloseSuppressed, "Restore should clear maintained corrections.");
            Require(
                level.WorkerSemanticLocation == "LOC_PT_A_WORKER_START",
                "Restore should return worker to authored start.");

            GD.Print("[PT_A_INTEGRATION] PASS");
            GetTree().Quit(0);
        }
        catch (Exception exception)
        {
            GD.PushError($"[PT_A_INTEGRATION] FAIL: {exception}");
            GetTree().Quit(1);
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
