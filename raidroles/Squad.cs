using System.Collections.Immutable;

class Squad {
    private Squad(IEnumerable<Member> firstSub, IEnumerable<Member> secondSub) {
        FirstSub = firstSub.ToImmutableArray();
        SecondSub = secondSub.ToImmutableArray();
    }

    public ImmutableArray<Member> FirstSub { get; }
    public ImmutableArray<Member> SecondSub { get; }

    public override string ToString() => 
        $"\n## Sub 1\n{string.Join('\n', FirstSub)}\n## Sub 2\n{string.Join('\n', SecondSub)}";

    public const int HEALER_SLOT = 0;
    public const int BOONER_SLOT = 1;
    public const int DPS_SLOTS_START = 2;
    public const int DPS_SLOTS_END = 4;

    public class Builder {
        private readonly Member[][] Subs = new Member[][] { new Member[5], new Member[5] };

        public Builder AddHealer(Applicant a, Role assignedRole) {
            if (!assignedRole.IsHeal()) throw new ArgumentException($"{assignedRole} is not a healer role");

            foreach (var sub in Subs) {
                if (sub[HEALER_SLOT] == null) {
                    sub[HEALER_SLOT] = new Member(a, assignedRole);
                    return this;
                }
            }

            throw new ArgumentException($"cannot place '{a}' first and second sub already have healers");
        }

        public Builder AddBoonDps(Applicant a, Role assignedRole) {
            if (!assignedRole.IsBoonDps()) throw new ArgumentException($"{assignedRole} is not a boon dps role");

            foreach (var sub in Subs) {
                if (sub[BOONER_SLOT] != null) continue;

                var subHealer = sub[HEALER_SLOT];
                if (subHealer == null || assignedRole == subHealer.AssignedRole.GetOppositeBoon()) {
                    sub[BOONER_SLOT] = new Member(a, assignedRole);
                    return this;
                }
            }

            throw new ArgumentException($"'{a}' with {assignedRole} does not fit into either sub");
        }

        public Builder AddDps(Applicant a) {
            foreach (var sub in Subs) {
                for (int i = DPS_SLOTS_START; i <= DPS_SLOTS_END; i++) {
                    if (sub[i] == null) {
                        sub[i] = new Member(a, Role.DPS);
                        return this;
                    }
                }
            }

            throw new ArgumentException($"cannot add '{a}'; squad is full");
        }

        public IEnumerable<Role> GetMissingRoles() {
            if (Subs[0][HEALER_SLOT] == null) {
                yield return Subs[0][BOONER_SLOT] != null
                    ? Subs[0][BOONER_SLOT].AssignedRole.GetOppositeBoon()
                    : Role.Q_HEAL | Role.A_HEAL;
            }

            if (Subs[1][HEALER_SLOT] == null) {
                yield return Subs[1][BOONER_SLOT] != null
                    ? Subs[1][BOONER_SLOT].AssignedRole.GetOppositeBoon()
                    : Role.Q_HEAL | Role.A_HEAL;
            }

            if (Subs[0][BOONER_SLOT] == null) {
                yield return Subs[0][HEALER_SLOT] != null
                    ? Subs[0][HEALER_SLOT].AssignedRole.GetOppositeBoon()
                    : Role.Q_DPS | Role.A_DPS;
            }

            if (Subs[1][BOONER_SLOT] == null) {
                yield return Subs[1][HEALER_SLOT] != null
                    ? Subs[1][HEALER_SLOT].AssignedRole.GetOppositeBoon()
                    : Role.Q_DPS | Role.A_DPS;
            }

            foreach (var sub in Subs) {
                for (int i = DPS_SLOTS_START; i <= DPS_SLOTS_END; i++) {
                    if (sub[i] == null) {
                        yield return Role.DPS;
                    }
                }
            }
        }

        public Squad Build() => new(Subs[0], Subs[1]);
    }
}