using Godot;

namespace Undo.Game.Debug;

public partial class BootstrapDebug : Node
{
    public override void _Ready()
    {
        GD.Print("[BOOTSTRAP] UNDO bootstrap scene ready.");
    }
}
