using System.Collections.Immutable;

class Squad {
    public ImmutableArray<Member> Sub1 { get; }
    public ImmutableArray<Member> Sub2 { get; }

    public override string ToString() => 
        $"\n## Sub 1\n{string.Join('\n', Sub1)}\n## Sub 2\n{string.Join('\n', Sub2)}";

    private Squad(IEnumerable<Member> firstSub, IEnumerable<Member> secondSub) {
        Sub1 = firstSub.ToImmutableArray();
        Sub2 = secondSub.ToImmutableArray();
    }

    private const int HEALER_SLOT = 0;
    private const int BOONER_SLOT = 1;
    private const int DPS_SLOTS_START = 2;
    private const int DPS_SLOTS_END = 4;

    public class Builder {
        private readonly Member[][] Subs = new Member[][] { new Member[5], new Member[5] };

        public Builder Add(Member m) {
            if (m.AssignedRole.IsHeal()) {
                AddHealer(m);
            } else if (m.AssignedRole.IsBoonDps()) {
                AddBoonDps(m);
            } else if (m.AssignedRole == Role.DPS) {
                AddDps(m);
            } else {
                throw new ArgumentException($"{m} does not have an assigned role");
            }

            return this;
        }

        public void AddHealer(Member m) {
            foreach (var sub in Subs) {
                if (sub[HEALER_SLOT] == null) {
                    sub[HEALER_SLOT] = m;
                    return;
                }
            }

            throw new ArgumentException($"cannot place {m} first and second sub already have healers");
        }

        public void AddBoonDps(Member m) {
            foreach (var sub in Subs) {
                if (sub[BOONER_SLOT] != null) continue;

                var subHealer = sub[HEALER_SLOT];
                if (subHealer == null || subHealer.AssignedRole.GetOppositeBoon().HasFlag(m.AssignedRole)) {
                    sub[BOONER_SLOT] = m;
                    return;
                }
            }

            throw new ArgumentException($"{m} does not fit into either sub");
        }

        public void AddDps(Member m) {
            foreach (var sub in Subs) {
                for (int i = DPS_SLOTS_START; i <= DPS_SLOTS_END; i++) {
                    if (sub[i] == null) {
                        sub[i] = m;
                        return;
                    }
                }
            }

            throw new ArgumentException($"cannot add {m}; squad is full");
        }

        public IEnumerable<(Role, int)> GetMissingRoles() {
            if (Subs[0][HEALER_SLOT] == null) {
                var role = Subs[0][BOONER_SLOT]?.AssignedRole.GetOppositeBoon() ?? Role.Q_HEAL | Role.A_HEAL;
                yield return (role, 1);
            }

            if (Subs[1][HEALER_SLOT] == null) {
                var role = Subs[1][BOONER_SLOT]?.AssignedRole.GetOppositeBoon() ?? Role.Q_HEAL | Role.A_HEAL;
                yield return (role, 1);
            }

            if (Subs[0][BOONER_SLOT] == null) {
                var role = Subs[0][HEALER_SLOT]?.AssignedRole.GetOppositeBoon() ?? Role.Q_DPS | Role.A_DPS;
                yield return (role, 1);
            }

            if (Subs[1][BOONER_SLOT] == null) {
                var role = Subs[1][HEALER_SLOT]?.AssignedRole.GetOppositeBoon() ?? Role.Q_DPS | Role.A_DPS;
                yield return (role, 1);
            }

            var count = 0;
            foreach (var sub in Subs) {
                for (int i = DPS_SLOTS_START; i <= DPS_SLOTS_END; i++) {
                    if (sub[i] == null) {
                        count++;
                    }
                }
            }
            yield return (Role.DPS, count);
        }

        public Squad Build() => new(Subs[0], Subs[1]);
    }
}