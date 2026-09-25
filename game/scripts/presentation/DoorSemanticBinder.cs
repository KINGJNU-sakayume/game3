using Godot;

namespace Undo.Game.Presentation;

public enum ResidualDoorState
{
    None,
    Open,
    Closed,
}

public partial class DoorSemanticBinder : Node3D
{
    [Export]
    public float OpenDegrees { get; set; } = -90.0f;

    [Export]
    public float TransitionSeconds { get; set; } = 0.45f;

    private Node3D _doorLeaf = null!;
    private Node3D _residualOpen = null!;
    private Node3D _residualClosed = null!;
    private float _targetYaw;
    private bool _moving;
    private bool _presentationPaused;
    private Action? _historicalCloseCommit;

    public override void _Ready()
    {
        _doorLeaf = GetNode<Node3D>("DoorLeaf");
        _residualOpen = GetNode<Node3D>("ResidualOpen");
        _residualClosed = GetNode<Node3D>("ResidualClosed");
        SetResidual(ResidualDoorState.None, visible: false);
    }

    public override void _Process(double delta)
    {
        if (_presentationPaused || !_moving)
        {
            return;
        }

        var fullTravel = Mathf.Abs(Mathf.DegToRad(OpenDegrees));
        var speed = fullTravel / Mathf.Max(TransitionSeconds, 0.01f);
        var rotation = _doorLeaf.Rotation;
        rotation.Y = Mathf.MoveToward(rotation.Y, _targetYaw, speed * (float)delta);
        _doorLeaf.Rotation = rotation;

        if (!Mathf.IsEqualApprox(rotation.Y, _targetYaw))
        {
            return;
        }

        _moving = false;

        if (_historicalCloseCommit is null || !Mathf.IsZeroApprox(_targetYaw))
        {
            return;
        }

        var callback = _historicalCloseCommit;
        _historicalCloseCommit = null;
        callback();
    }

    public void BeginHistoricalClose(Action onLatched)
    {
        _historicalCloseCommit = onLatched ?? throw new ArgumentNullException(nameof(onLatched));
        SetTargetOpen(isOpen: false, immediate: false);
    }

    public void ApplyEffectiveOpen(bool isOpen, bool immediate = false)
    {
        _historicalCloseCommit = null;
        SetTargetOpen(isOpen, immediate);
    }

    public void ResetToInitialOpen()
    {
        _historicalCloseCommit = null;
        SetTargetOpen(isOpen: true, immediate: true);
        SetResidual(ResidualDoorState.None, visible: false);
    }

    public void SetPresentationPaused(bool paused) => _presentationPaused = paused;

    public void SetResidual(ResidualDoorState state, bool visible)
    {
        _residualOpen.Visible = visible && state == ResidualDoorState.Open;
        _residualClosed.Visible = visible && state == ResidualDoorState.Closed;
    }

    private void SetTargetOpen(bool isOpen, bool immediate)
    {
        _targetYaw = isOpen ? Mathf.DegToRad(OpenDegrees) : 0.0f;

        if (!immediate)
        {
            _moving = true;
            return;
        }

        var rotation = _doorLeaf.Rotation;
        rotation.Y = _targetYaw;
        _doorLeaf.Rotation = rotation;
        _moving = false;
    }
}
