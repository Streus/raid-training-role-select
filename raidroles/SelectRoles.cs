using static Role;

partial class Program {
    static void SelectRoles(HashSet<Applicant> applicants) {
        var squad = new Squad.Builder();
        var remainingApplicants = applicants;

        remainingApplicants = PlaceTrainers(remainingApplicants, ref squad);

        remainingApplicants = PlaceGuaranteeds(remainingApplicants, ref squad);

        remainingApplicants = FillSquad(remainingApplicants, ref squad);

        remainingApplicants = DesignateNextGuaranteeds(remainingApplicants);

        var finalSquad = squad.Build();

        Console.WriteLine(finalSquad);
        if (remainingApplicants.Count > 0) {
            Console.WriteLine($"\n## Guaranteed for next week\n{string.Join('\n', remainingApplicants.Select(a => a.RenderedId))}");
        }
        Console.WriteLine($"\nSeed: {Seed:X8}");
        Console.WriteLine("Review the source at https://github.com/Streus/raid-training-role-select");
    }

    private static HashSet<Applicant> PlaceTrainers(HashSet<Applicant> remainingApplicants, ref Squad.Builder squad) {
        var trainerApplicants = remainingApplicants
            .Where(a => a.IsTrainer)
            .OrderBy(_ => Random.Next());

        if (trainerApplicants.Any()) {
            Console.WriteLine($"\n----- Placing {trainerApplicants.Count()} trainers -----\n");
            
            foreach (var a in trainerApplicants) {
                Console.WriteLine($"Placing {a.RenderedId} as {a.PrimaryRole.GetPrettyName()}");

                squad.Add(new Member(a, a.PrimaryRole));
            }
        }

        return remainingApplicants
            .Except(trainerApplicants)
            .ToHashSet();
    }

    private static HashSet<Applicant> PlaceGuaranteeds(HashSet<Applicant> remainingApplicants, ref Squad.Builder squad) {
        var guaranteedApplicants = remainingApplicants
            .Where(a => a.IsGuaranteed)
            .OrderBy(_ => Random.Next());

        if (guaranteedApplicants.Any()) {
            Console.WriteLine($"\n----- Placing {guaranteedApplicants.Count()} guaranteeds -----\n");

            foreach (var a in guaranteedApplicants) {
                Console.WriteLine($"Placing {a.RenderedId} as {a.PrimaryRole.GetPrettyName()}");

                squad.Add(new Member(a, a.PrimaryRole));
            }
        }

        return remainingApplicants
            .Except(guaranteedApplicants)
            .ToHashSet();
    }

    private static HashSet<Applicant> FillSquad(HashSet<Applicant> remainingApplicants, ref Squad.Builder squad) {
        HashSet<Applicant> leftover = remainingApplicants;
        foreach (var (role, count) in squad.GetMissingRoles()) {
            Console.WriteLine($"\n----- Selecting for {count} {role.GetPrettyName()} -----\n");

            var prospectives = leftover
                .Where(a => a.PrimaryRole != NONE && role.HasFlag(a.PrimaryRole))
                .OrderBy(_ => Random.Next())
                .ToList();

            var backupProspectives = leftover
                .Except(prospectives)
                .Where(a => a.BackupRoles != NONE && role.HasFlag(a.BackupRoles))
                .OrderBy(_ => Random.Next())
                .AsEnumerable();

            prospectives.AddRange(backupProspectives);

            if (prospectives.Any()) {
                Console.WriteLine($"In drawing:\n{string.Join('\n', prospectives.Select(p => p.RenderedId))}\n");
            }

            foreach (var confirmed in prospectives.Take(count)) {
                var assignedRole = confirmed.PrimaryRole != NONE && role.HasFlag(confirmed.PrimaryRole)
                    ? confirmed.PrimaryRole
                    : role.Pin(Random);
                
                Console.WriteLine($"Placing {confirmed.RenderedId} as {assignedRole.GetPrettyName()}");

                squad.Add(new Member(confirmed, assignedRole));

                leftover = leftover
                    .Except(new Applicant[] { confirmed })
                    .ToHashSet();
            }
            
            int numMissing = Math.Max(0, count - prospectives.Count);
            for (int i = 0; i < numMissing; i++) {
                squad.Add(new Member(role));
            }
        }
        
        return leftover;
    }

    private static HashSet<Applicant> DesignateNextGuaranteeds(HashSet<Applicant> remainingApplicants) {
        return remainingApplicants
            .Where(a => a.PrimaryRole != NONE)
            .ToHashSet();
    }
}
