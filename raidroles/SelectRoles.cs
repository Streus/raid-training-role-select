using static Role;

partial class Program {
    static void SelectRoles(HashSet<Applicant> applicants) {
        var squad = new Squad.Builder();
        var remainingApplicants = applicants;
        
        // ----- Place trainers ----- //
        var trainerApplicants = remainingApplicants
            .Where(a => a.IsTrainer)
            .OrderBy(a => Random.Next());

        Console.WriteLine($"\n----- Placing {trainerApplicants.Count()} trainers -----\n");
        
        foreach (var a in trainerApplicants) {
            if ((Q_HEAL | A_HEAL).HasFlag(a.PrimaryRole)) {
                squad.AddHealer(a, a.PrimaryRole);
            } else if ((Q_DPS | A_DPS).HasFlag(a.PrimaryRole)) {
                squad.AddBoonDps(a, a.PrimaryRole);
            } else if (a.PrimaryRole == DPS) {
                squad.AddDps(a);
            }
        }

        remainingApplicants = remainingApplicants
            .Except(trainerApplicants)
            .ToHashSet();

        // ----- Place guaranteeds ----- //
        var guaranteedApplicants = remainingApplicants
            .Where(a => a.IsGuaranteed)
            .OrderBy(a => Random.Next());

        if (guaranteedApplicants.Count() > 0) {
            Console.WriteLine($"\n----- Placing {guaranteedApplicants.Count()} guaranteeds -----\n");

            foreach (var a in guaranteedApplicants) {
                if ((Q_HEAL | A_HEAL).HasFlag(a.PrimaryRole)) {
                    squad.AddHealer(a, a.PrimaryRole);
                } else if ((Q_DPS | A_DPS).HasFlag(a.PrimaryRole)) {
                    squad.AddBoonDps(a, a.PrimaryRole);
                } else if (a.PrimaryRole == DPS) {
                    squad.AddDps(a);
                }
            }

            remainingApplicants = remainingApplicants
                .Except(guaranteedApplicants)
                .ToHashSet();
        }

        // ----- Select for any remaining open roles -----
        foreach (var role in squad.GetMissingRoles()) {
            Console.WriteLine($"\n----- Selecting for {role} -----\n");

            var prospectives = remainingApplicants
                .Where(a => role.HasFlag(a.PrimaryRole))
                .OrderBy(a => Random.Next())
                .ToList();

            var backupProspectives = remainingApplicants
                .Except(prospectives)
                .Where(a => a.BackupRoles.HasFlag(role))
                .OrderBy(a => Random.Next())
                .AsEnumerable();

            prospectives.AddRange(backupProspectives);

            Console.WriteLine($"In drawing:\n{string.Join('\n', prospectives.Select(p => p.RenderedId))}\n");

            var prospective = prospectives[0];

            var assignedRole = role.HasFlag(prospective.PrimaryRole)
                ? prospective.PrimaryRole
                : role;

            Console.WriteLine($"Placing {prospective.RenderedId} as {assignedRole.GetPrettyName()}");

            if (role.IsHeal()) {
                squad.AddHealer(prospective, assignedRole);
            } else if (role.IsBoonDps()) {
                squad.AddBoonDps(prospective, assignedRole);
            } else {
                squad.AddDps(prospective);
            }

            remainingApplicants = remainingApplicants
                .Except(new Applicant[] { prospective })
                .ToHashSet();
        }

        var finalSquad = squad.Build();

        Console.WriteLine($"\n## Sub 1\n{string.Join('\n', finalSquad.FirstSub)}");
        Console.WriteLine($"\n## Sub 2\n{string.Join('\n', finalSquad.SecondSub)}");
        if (remainingApplicants.Count > 0) {
            Console.WriteLine($"\n## Guaranteed for next week\n{string.Join('\n', remainingApplicants.Select(a => a.RenderedId))}");
        }
        Console.WriteLine($"\n*Seed: {Seed:X8}*");
        Console.WriteLine("*Review the source at https://github.com/Streus/raid-training-role-select*");
    }
}
