using static Role;

static class RoleExtensions {
    public static bool IsHeal(this Role r) => r.HasFlag(Q_HEAL) || r.HasFlag(A_HEAL);
    public static bool IsBoonDps(this Role r) => r.HasFlag(Q_DPS) || r.HasFlag(A_DPS);
    public static bool IsQuick(this Role r) => r.HasFlag(Q_HEAL) || r.HasFlag(Q_DPS);
    public static bool IsAlac(this Role r) => r.HasFlag(A_HEAL) || r.HasFlag(A_DPS);
    public static Role GetOppositeBoon(this Role r) => r switch {
        Q_HEAL => A_DPS,
        A_HEAL => Q_DPS,
        Q_HEAL | A_HEAL => A_DPS | Q_DPS,
        Q_DPS => A_HEAL,
        A_DPS => Q_HEAL,
        Q_DPS | A_DPS => A_HEAL | Q_HEAL,
        _ => throw new ArgumentException($"{r} is not a boon role")
    };

    public static IEnumerable<Role> Decompose(this Role r) {
        List<Role> decomposed = new();
        foreach (Role single in Enum.GetValues<Role>().Skip(1)) {
            if (r.HasFlag(single)) decomposed.Add(single);
        }
        return decomposed;
    }

    public static Role Pin(this Role r, Random random) => r.Decompose().OrderBy(_ => random.Next()).FirstOrDefault(DPS);

    public static string GetPrettyName(this Role r) => r switch {
        Q_HEAL => "Quick Heal",
        A_HEAL => "Alac Heal",
        Q_HEAL | A_HEAL => "Quick/Alac Heal",
        Q_DPS => "Quick DPS",
        A_DPS => "Alac DPS",
        Q_DPS | A_DPS => "Quick/Alac DPS",
        DPS => "DPS",
        _ => "None",
    };
}
