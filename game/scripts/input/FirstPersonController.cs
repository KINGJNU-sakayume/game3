using Godot;

namespace Undo.Game.Input;

public partial class FirstPersonController : CharacterBody3D
{
    [Export]
    public float MoveSpeed { get; set; } = 3.6f;

    [Export]
    public float MouseSensitivity { get; set; } = 0.0025f;

    private Camera3D _camera = null!;
    private Transform3D _initialTransform;
    private Vector3 _initialCameraRotation;
    private bool _movementEnabled = true;

    public Camera3D Camera => _camera;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        _initialTransform = GlobalTransform;
        _initialCameraRotation = _camera.Rotation;
        SetMouseCaptured(true);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!_movementEnabled ||
            Godot.Input.MouseMode != Godot.Input.MouseModeEnum.Captured ||
            @event is not InputEventMouseMotion mouseMotion)
        {
            return;
        }

        RotateY(-mouseMotion.Relative.X * MouseSensitivity);

        var cameraRotation = _camera.Rotation;
        cameraRotation.X = Mathf.Clamp(
            cameraRotation.X - (mouseMotion.Relative.Y * MouseSensitivity),
            Mathf.DegToRad(-85.0f),
            Mathf.DegToRad(85.0f));
        _camera.Rotation = cameraRotation;

        GetViewport().SetInputAsHandled();
    }

    public override void _PhysicsProcess(double delta)
    {
        _ = delta;

        if (!_movementEnabled)
        {
            Velocity = Vector3.Zero;
            return;
        }

        var movementInput = Godot.Input.GetVector(
            "move_left",
            "move_right",
            "move_forward",
            "move_back");

        var localDirection = new Vector3(movementInput.X, 0.0f, movementInput.Y);
        var worldDirection = GlobalTransform.Basis * localDirection;
        worldDirection.Y = 0.0f;

        if (worldDirection.LengthSquared() > 1.0f)
        {
            worldDirection = worldDirection.Normalized();
        }

        Velocity = worldDirection * MoveSpeed;
        MoveAndSlide();
    }

    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
        if (!enabled)
        {
            Velocity = Vector3.Zero;
        }
    }

    public void SetMouseCaptured(bool captured)
    {
        Godot.Input.MouseMode = captured
            ? Godot.Input.MouseModeEnum.Captured
            : Godot.Input.MouseModeEnum.Visible;
    }

    public void RestoreInitialState()
    {
        GlobalTransform = _initialTransform;
        _camera.Rotation = _initialCameraRotation;
        Velocity = Vector3.Zero;
    }
}
