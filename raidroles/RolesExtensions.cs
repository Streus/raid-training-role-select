static class RoleExtensions {
    public static bool IsHeal(this Role r) => r.HasFlag(Role.Q_HEAL) || r.HasFlag(Role.A_HEAL);
    public static bool IsBoonDps(this Role r) => r.HasFlag(Role.Q_DPS) || r.HasFlag(Role.A_DPS);
    public static bool IsQuick(this Role r) => r.HasFlag(Role.Q_HEAL) || r.HasFlag(Role.Q_DPS);
    public static bool IsAlac(this Role r) => r.HasFlag(Role.A_HEAL) || r.HasFlag(Role.A_DPS);
    public static Role GetOppositeBoon(this Role r) => r.HasFlag(Role.Q_HEAL)? Role.A_DPS : r.HasFlag(Role.A_HEAL)? Role.Q_DPS : Role.NONE;

    public static string GetPrettyName(this Role r) {
        return r switch {
            Role.Q_HEAL => "Quick Heal",
            Role.A_HEAL => "Alac Heal",
            Role.Q_DPS => "Quick DPS",
            Role.A_DPS => "Alac DPS",
            Role.DPS => "DPS",
            _ => "None",
        };
    }
}
