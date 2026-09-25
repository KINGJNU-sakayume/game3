namespace Undo.Core.State;

public static class StateValueRules
{
    public static bool IsCompatible(StateChannel channel, StateValueKind kind) =>
        channel switch
        {
            StateChannel.Open => kind == StateValueKind.Boolean,
            StateChannel.Lock => kind == StateValueKind.Identifier,
            StateChannel.Position => kind == StateValueKind.Identifier,
            StateChannel.Possession => kind == StateValueKind.Identifier,
            StateChannel.Authorization => kind == StateValueKind.Identifier,
            StateChannel.Power => kind is StateValueKind.Boolean or StateValueKind.Identifier,
            _ => kind is StateValueKind.Boolean or StateValueKind.Integer or StateValueKind.Identifier,
        };
}
