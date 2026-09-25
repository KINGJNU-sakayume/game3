using Godot;

namespace Undo.Game.Application;

public partial class PrototypeAWorker : Node3D
{
    [Export]
    public float MoveSpeed { get; set; } = 1.5f;

    public event Action<PrototypeARoutineStepContent>? IntentRequested;

    private IReadOnlyList<PrototypeARoutineStepContent> _routine =
        Array.Empty<PrototypeARoutineStepContent>();

    private IReadOnlyDictionary<string, Vector3> _locations =
        new Dictionary<string, Vector3>(StringComparer.Ordinal);

    private string _initialLocation = string.Empty;
    private int _stepIndex;
    private bool _intentPending;
    private bool _routinePaused;
    private int _waitStepIndex = -1;
    private double _waitRemaining;

    public string CurrentSemanticLocation { get; private set; } = string.Empty;

    public int RoutineStepIndex => _stepIndex;

    public bool RoutineComplete => _stepIndex >= _routine.Count;

    public string RoutineStepDescription
    {
        get
        {
            if (RoutineComplete)
            {
                return "COMPLETE";
            }

            var step = _routine[_stepIndex];
            return step.Type switch
            {
                "MOVE_TO" => $"MOVE_TO {step.Location}",
                "WAIT" => $"WAIT {step.DurationSeconds:0.0}s",
                "INTENT" => $"{step.Verb} {step.Target}",
                _ => step.Type,
            };
        }
    }

    public void Configure(
        PrototypeAActorContent actor,
        IReadOnlyDictionary<string, Vector3> locations)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(locations);

        _routine = actor.Routine;
        _locations = locations;
        _initialLocation = actor.InitialLocation;

        ResetRoutine();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_routinePaused || RoutineComplete)
        {
            return;
        }

        var step = _routine[_stepIndex];

        switch (step.Type)
        {
            case "MOVE_TO":
                ProcessMoveStep(step, delta);
                break;
            case "WAIT":
                ProcessWaitStep(step, delta);
                break;
            case "INTENT":
                ProcessIntentStep(step);
                break;
            default:
                throw new InvalidOperationException($"Unsupported routine step type '{step.Type}'.");
        }
    }

    public void CompleteIntent()
    {
        if (!_intentPending)
        {
            throw new InvalidOperationException("No Prototype A intent is awaiting completion.");
        }

        _intentPending = false;
        _stepIndex++;
    }

    public void SetRoutinePaused(bool paused) => _routinePaused = paused;

    public void ResetRoutine()
    {
        if (!_locations.TryGetValue(_initialLocation, out var startPosition))
        {
            throw new InvalidOperationException($"Unknown worker initial location '{_initialLocation}'.");
        }

        GlobalPosition = startPosition;
        CurrentSemanticLocation = _initialLocation;
        _stepIndex = 0;
        _intentPending = false;
        _routinePaused = false;
        _waitStepIndex = -1;
        _waitRemaining = 0.0;
    }

    private void ProcessMoveStep(PrototypeARoutineStepContent step, double delta)
    {
        if (step.Location is null || !_locations.TryGetValue(step.Location, out var target))
        {
            throw new InvalidOperationException($"Unknown MOVE_TO location '{step.Location}'.");
        }

        GlobalPosition = GlobalPosition.MoveToward(target, MoveSpeed * (float)delta);

        if (GlobalPosition.DistanceTo(target) > 0.025f)
        {
            return;
        }

        GlobalPosition = target;
        CurrentSemanticLocation = step.Location;
        _stepIndex++;
    }

    private void ProcessWaitStep(PrototypeARoutineStepContent step, double delta)
    {
        if (_waitStepIndex != _stepIndex)
        {
            _waitStepIndex = _stepIndex;
            _waitRemaining = step.DurationSeconds ?? 0.0;
        }

        _waitRemaining -= delta;
        if (_waitRemaining > 0.0)
        {
            return;
        }

        _waitStepIndex = -1;
        _waitRemaining = 0.0;
        _stepIndex++;
    }

    private void ProcessIntentStep(PrototypeARoutineStepContent step)
    {
        if (_intentPending)
        {
            return;
        }

        _intentPending = true;
        IntentRequested?.Invoke(step);
    }
}
