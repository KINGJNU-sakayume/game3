using System.Text;
using System.Text.Json;
using Godot;
using Undo.Core;
using Undo.Core.Events;
using Undo.Core.Puzzles;
using Undo.Core.State;
using Undo.Game.Input;
using Undo.Game.Presentation;

namespace Undo.Game.Application;

public partial class PrototypeALevel : Node3D
{
    private const string ContentPath = "res://content/puzzles/PT_A_BASIC_CORRECTION/puzzle.json";
    private const string PrototypeId = "PT_A_BASIC_CORRECTION";
    private const string DoorIdValue = "DOOR_A";
    private const string WorkerIdValue = "NPC_WORKER_01";
    private const string CloseEventIdValue = "E1";
    private const string ExitLocationId = "LOC_PT_A_EXIT";

    private readonly EntityId _doorId = new(DoorIdValue);
    private readonly EntityId _workerId = new(WorkerIdValue);
    private readonly EventId _closeEventId = new(CloseEventIdValue);

    private PrototypeAPuzzleContent _content = null!;
    private PuzzleSession _session = null!;
    private StateKey _doorOpenKey;

    private FirstPersonController _player = null!;
    private PrototypeAWorker _worker = null!;
    private DoorSemanticBinder _door = null!;
    private RayCast3D _reviewRay = null!;
    private Label _reviewLabel = null!;
    private Label _debugLabel = null!;
    private Label _completeLabel = null!;
    private Control _pauseOverlay = null!;
    private Label _pauseTitle = null!;
    private Button _continueButton = null!;
    private Button _restoreButton = null!;
    private Button _exitButton = null!;

    private IReadOnlyDictionary<string, Vector3> _locations =
        new Dictionary<string, Vector3>(StringComparer.Ordinal);

    private bool _reviewActive;
    private bool _menuOpen;
    private bool _completed;

    public bool DoorEffectiveOpen => _session.Resolve(_doorOpenKey).AsBoolean();

    public int RecordedEventCount => _session.EventLedger.Events.Count;

    public bool CloseSuppressed => _session.Corrections.IsSuppressed(_closeEventId);

    public string WorkerSemanticLocation => _worker.CurrentSemanticLocation;

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>("%Player");
        _worker = GetNode<PrototypeAWorker>("%Worker");
        _door = GetNode<DoorSemanticBinder>("%DoorA");
        _reviewRay = GetNode<RayCast3D>("%ReviewRay");
        _reviewLabel = GetNode<Label>("%ReviewLabel");
        _debugLabel = GetNode<Label>("%DebugLabel");
        _completeLabel = GetNode<Label>("%CompleteLabel");
        _pauseOverlay = GetNode<Control>("%PauseOverlay");
        _pauseTitle = GetNode<Label>("%PauseTitle");
        _continueButton = GetNode<Button>("%ContinueButton");
        _restoreButton = GetNode<Button>("%RestoreButton");
        _exitButton = GetNode<Button>("%ExitButton");

        _content = PrototypeAPuzzleContent.Load(ContentPath);
        if (!string.Equals(_content.PuzzleId, PrototypeId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Expected puzzle '{PrototypeId}', loaded '{_content.PuzzleId}'.");
        }

        _locations = ReadSceneLocations();
        ValidateRequiredLocations();

        var worldState = BuildInitialWorldState(_content);
        _doorOpenKey = new StateKey(_doorId, StateChannel.Open);
        _session = new PuzzleSession(worldState, _content.CorrectionCapacity);

        var workerContent = _content.Actors.Single(actor =>
            string.Equals(actor.ActorId, WorkerIdValue, StringComparison.Ordinal));

        _worker.Configure(workerContent, _locations);
        _worker.IntentRequested += OnWorkerIntentRequested;

        _door.ApplyEffectiveOpen(DoorEffectiveOpen, immediate: true);

        _continueButton.Pressed += ClosePauseMenu;
        _restoreButton.Pressed += OnRestorePressed;
        _exitButton.Pressed += OnExitPressed;

        _pauseTitle.Text = Localized("UI.PAUSE.TITLE", "Paused");
        _continueButton.Text = Localized("UI.PAUSE.CONTINUE", "Continue");
        _restoreButton.Text = Localized("UI.PAUSE.RESTORE_RECORD", "Restore Current Record");
        _exitButton.Text = Localized("UI.PAUSE.EXIT", "Exit");
        _completeLabel.Text = Localized("UI.PROTO_A.COMPLETE", "Prototype A complete");

        _pauseOverlay.Visible = false;
        _debugLabel.Visible = false;
        _completeLabel.Visible = false;
        _reviewLabel.Text = string.Empty;

        GD.Print($"[PUZZLE] {PrototypeId} ready capacity={_content.CorrectionCapacity}");
    }

    public override void _ExitTree()
    {
        if (_worker is not null)
        {
            _worker.IntentRequested -= OnWorkerIntentRequested;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            if (_menuOpen)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }

            GetViewport().SetInputAsHandled();
            return;
        }

        if (@event is InputEventKey key &&
            key.Pressed &&
            !key.Echo &&
            (key.Keycode == Key.F3 || key.PhysicalKeycode == Key.F3))
        {
            _debugLabel.Visible = !_debugLabel.Visible;
            GetViewport().SetInputAsHandled();
        }
    }

    public override void _Process(double delta)
    {
        _ = delta;

        UpdateReview();
        UpdateCompletion();
        UpdateDebugDisplay();
    }

    public bool CommitCloseCorrection()
    {
        if (!HasCloseEvent || CloseSuppressed)
        {
            return false;
        }

        try
        {
            _session.CorrectionManager.Suppress(_closeEventId);
        }
        catch (InvalidOperationException exception)
        {
            GD.PrintErr($"[CORRECTION] suppress rejected event={_closeEventId} reason={exception.Message}");
            return false;
        }

        ApplyResolvedDoorState();
        GD.Print($"[CORRECTION] suppress {_closeEventId}");
        GD.Print($"[STATE] {DoorIdValue}.OPEN -> {DoorEffectiveOpen}");
        return true;
    }

    public bool ReleaseCloseCorrection()
    {
        if (!HasCloseEvent || !CloseSuppressed)
        {
            return false;
        }

        try
        {
            _session.CorrectionManager.Release(_closeEventId);
        }
        catch (InvalidOperationException exception)
        {
            GD.PrintErr($"[CORRECTION] release rejected event={_closeEventId} reason={exception.Message}");
            return false;
        }

        ApplyResolvedDoorState();
        GD.Print($"[CORRECTION] release {_closeEventId}");
        GD.Print($"[STATE] {DoorIdValue}.OPEN -> {DoorEffectiveOpen}");
        return true;
    }

    public void RestoreCurrentRecord()
    {
        _reviewActive = false;
        _completed = false;

        _session.RestoreCurrentRecord();
        _door.ResetToInitialOpen();
        _worker.ResetRoutine();
        _player.RestoreInitialState();

        _completeLabel.Visible = false;
        _reviewLabel.Text = string.Empty;
        _door.SetResidual(ResidualDoorState.None, visible: false);

        ApplyInteractionPause();
        GD.Print($"[RESTORE] {PrototypeId} restored to authored snapshot.");
    }

    private bool HasCloseEvent =>
        _session.EventLedger.TryGet(_closeEventId, out _);

    private void OnWorkerIntentRequested(PrototypeARoutineStepContent step)
    {
        if (!string.Equals(step.Type, "INTENT", StringComparison.Ordinal) ||
            !string.Equals(step.Verb, "CLOSE", StringComparison.Ordinal) ||
            !string.Equals(step.Target, DoorIdValue, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Prototype A received unsupported intent '{step.Verb}' target='{step.Target}'.");
        }

        GD.Print($"[ACTOR] {WorkerIdValue} intent=CLOSE target={DoorIdValue}");

        _door.BeginHistoricalClose(() => CommitHistoricalClose(step));
    }

    private void CommitHistoricalClose(PrototypeARoutineStepContent step)
    {
        if (HasCloseEvent)
        {
            throw new InvalidOperationException("Prototype A CLOSE event was already recorded.");
        }

        var source = ParseRecordSource(step.Source);

        var recordedEvent = _session.EventRecorder.Record(
            _closeEventId,
            DateTimeOffset.UtcNow,
            _workerId,
            EventVerb.Close,
            _doorId,
            [new StateChange(_doorOpenKey, StateValue.Boolean(false))],
            step.Reversible,
            source);

        _door.ApplyEffectiveOpen(DoorEffectiveOpen, immediate: true);
        _worker.CompleteIntent();

        GD.Print(
            $"[EVENT] {recordedEvent.Id} seq={recordedEvent.Sequence} actor={recordedEvent.ActorId} verb={recordedEvent.Verb} target={recordedEvent.TargetId}");
        GD.Print($"[STATE] {DoorIdValue}.OPEN True -> {DoorEffectiveOpen}");
    }

    private void UpdateReview()
    {
        if (_menuOpen)
        {
            if (_reviewActive)
            {
                _reviewActive = false;
                ApplyInteractionPause();
            }

            _reviewLabel.Text = string.Empty;
            UpdateResidual(targeted: false);
            return;
        }

        var targeted = HasCloseEvent && IsDoorTargeted();
        var wantsReview = targeted && Godot.Input.IsActionPressed("review");

        if (wantsReview != _reviewActive)
        {
            _reviewActive = wantsReview;
            ApplyInteractionPause();

            if (_reviewActive)
            {
                GD.Print($"[REVIEW] target={DoorIdValue} event={CloseEventIdValue}");
            }
        }

        if (_reviewActive && !CloseSuppressed &&
            Godot.Input.IsActionJustPressed("correction_commit"))
        {
            CommitCloseCorrection();
        }
        else if (_reviewActive && CloseSuppressed &&
                 Godot.Input.IsActionJustPressed("correction_release"))
        {
            ReleaseCloseCorrection();
        }

        UpdateResidual(targeted);
        UpdateReviewLabel(targeted);
    }

    private void UpdateResidual(bool targeted)
    {
        if (!HasCloseEvent)
        {
            _door.SetResidual(ResidualDoorState.None, visible: false);
            return;
        }

        if (CloseSuppressed)
        {
            _door.SetResidual(ResidualDoorState.Closed, visible: true);
            return;
        }

        _door.SetResidual(ResidualDoorState.Open, visible: targeted);
    }

    private void UpdateReviewLabel(bool targeted)
    {
        if (_reviewActive)
        {
            if (CloseSuppressed)
            {
                _reviewLabel.Text =
                    Localized("UI.PROTO_A.STATE.CLOSE_SUPPRESSED", "Close record suppressed") +
                    "\n" +
                    Localized("UI.PROTO_A.ACTION.RELEASE", "Release correction [Q]");
            }
            else
            {
                _reviewLabel.Text =
                    Localized("UI.PROTO_A.STATE.CLOSED", "Closed") +
                    "\n" +
                    Localized("UI.PROTO_A.ACTION.COMMIT", "Commit correction [LMB]");
            }

            return;
        }

        if (!targeted)
        {
            _reviewLabel.Text = string.Empty;
            return;
        }

        _reviewLabel.Text = CloseSuppressed
            ? Localized(
                "UI.PROTO_A.PROMPT.MAINTAINED",
                "Correction maintained · Review [RMB]")
            : Localized("UI.PROTO_A.PROMPT.REVIEW", "Review [RMB]");
    }

    private bool IsDoorTargeted()
    {
        _reviewRay.ForceRaycastUpdate();
        if (!_reviewRay.IsColliding())
        {
            return false;
        }

        return _reviewRay.GetCollider() is Node node &&
               node.IsInGroup("review_target_door");
    }

    private void UpdateCompletion()
    {
        if (_completed || !_locations.TryGetValue(ExitLocationId, out var exitPosition))
        {
            return;
        }

        var playerPosition = _player.GlobalPosition;
        playerPosition.Y = exitPosition.Y;

        if (playerPosition.DistanceTo(exitPosition) > 0.8f)
        {
            return;
        }

        _completed = true;
        _completeLabel.Visible = true;
        GD.Print($"[PUZZLE] {PrototypeId} completion playerAt={ExitLocationId}");
    }

    private void OpenPauseMenu()
    {
        _menuOpen = true;
        _reviewActive = false;
        _pauseOverlay.Visible = true;
        ApplyInteractionPause();
        _player.SetMouseCaptured(false);
    }

    private void ClosePauseMenu()
    {
        _menuOpen = false;
        _pauseOverlay.Visible = false;
        ApplyInteractionPause();
        _player.SetMouseCaptured(true);
    }

    private void OnRestorePressed()
    {
        RestoreCurrentRecord();
        ClosePauseMenu();
    }

    private void OnExitPressed() => GetTree().Quit();

    private void ApplyInteractionPause()
    {
        var actorPaused = _menuOpen || _reviewActive;
        _player.SetMovementEnabled(!actorPaused);
        _worker.SetRoutinePaused(actorPaused);
        _door.SetPresentationPaused(_menuOpen);
    }

    private void ApplyResolvedDoorState() =>
        _door.ApplyEffectiveOpen(DoorEffectiveOpen, immediate: false);

    private void UpdateDebugDisplay()
    {
        if (!_debugLabel.Visible)
        {
            return;
        }

        var builder = new StringBuilder();
        builder.AppendLine("DEV DEBUG — PT_A_BASIC_CORRECTION");
        builder.AppendLine($"CorrectionCapacity = {_session.Corrections.Capacity}");
        builder.AppendLine($"DOOR_A.OPEN = {DoorEffectiveOpen}");
        builder.AppendLine($"Suppressed = {(CloseSuppressed ? CloseEventIdValue : "none")}");
        builder.AppendLine("EventLedger:");

        if (_session.EventLedger.Events.Count == 0)
        {
            builder.AppendLine("  <empty>");
        }
        else
        {
            foreach (var recordedEvent in _session.EventLedger.Events)
            {
                builder.AppendLine(
                    $"  {recordedEvent.Id} seq={recordedEvent.Sequence} {recordedEvent.ActorId} {recordedEvent.Verb} {recordedEvent.TargetId}");
            }
        }

        builder.AppendLine($"NPC_WORKER_01 location = {_worker.CurrentSemanticLocation}");
        builder.AppendLine(
            $"NPC_WORKER_01 routine = {_worker.RoutineStepIndex}: {_worker.RoutineStepDescription}");
        builder.AppendLine($"Review = {_reviewActive}");
        builder.AppendLine("F3 = toggle debug");

        _debugLabel.Text = builder.ToString();
    }

    private IReadOnlyDictionary<string, Vector3> ReadSceneLocations()
    {
        var locationRoot = GetNode<Node3D>("%Locations");
        var result = new Dictionary<string, Vector3>(StringComparer.Ordinal);

        foreach (var child in locationRoot.GetChildren())
        {
            if (child is not Marker3D marker)
            {
                continue;
            }

            var locationId = marker.Name.ToString();
            if (!result.TryAdd(locationId, marker.GlobalPosition))
            {
                throw new InvalidOperationException($"Duplicate scene LocationId '{locationId}'.");
            }
        }

        return result;
    }

    private void ValidateRequiredLocations()
    {
        foreach (var locationId in _content.Locations)
        {
            if (!_locations.ContainsKey(locationId))
            {
                throw new InvalidOperationException(
                    $"Scene is missing required semantic location '{locationId}'.");
            }
        }
    }

    private static WorldStateStore BuildInitialWorldState(PrototypeAPuzzleContent content)
    {
        var worldState = new WorldStateStore();

        foreach (var (entityIdValue, channels) in content.InitialState)
        {
            var entityId = new EntityId(entityIdValue);

            foreach (var (channelName, jsonValue) in channels)
            {
                if (!Enum.TryParse<StateChannel>(channelName, ignoreCase: true, out var channel))
                {
                    throw new InvalidOperationException(
                        $"Content contains unknown state channel '{channelName}'.");
                }

                var value = ParseStateValue(jsonValue);
                if (!StateValueRules.IsCompatible(channel, value.Kind))
                {
                    throw new InvalidOperationException(
                        $"Content value for {entityIdValue}.{channelName} is incompatible with the channel.");
                }

                worldState.AddInitial(new StateKey(entityId, channel), value);
            }
        }

        return worldState;
    }

    private static StateValue ParseStateValue(JsonElement value) =>
        value.ValueKind switch
        {
            JsonValueKind.True => StateValue.Boolean(true),
            JsonValueKind.False => StateValue.Boolean(false),
            JsonValueKind.String => StateValue.Identifier(
                value.GetString() ??
                throw new InvalidOperationException("Identifier state value cannot be null.")),
            JsonValueKind.Number when value.TryGetInt64(out var integer) =>
                StateValue.Integer(integer),
            _ => throw new InvalidOperationException(
                $"Unsupported state value JSON kind '{value.ValueKind}'."),
        };

    private static RecordSource ParseRecordSource(string? source) =>
        source switch
        {
            "ACCESS_CONTROL" => RecordSource.AccessControl,
            "MACHINERY" => RecordSource.Machinery,
            "SECURITY" => RecordSource.Security,
            "ARCHIVE_SYSTEM" => RecordSource.ArchiveSystem,
            "ENVIRONMENTAL_SENSOR" => RecordSource.EnvironmentalSensor,
            "CORRELATED_INSTITUTIONAL_RECORD" => RecordSource.CorrelatedInstitutionalRecord,
            _ => throw new InvalidOperationException($"Unknown record source '{source}'."),
        };

    private string Localized(string key, string fallback)
    {
        var translated = Tr(key);
        return string.Equals(translated, key, StringComparison.Ordinal)
            ? fallback
            : translated;
    }
}
