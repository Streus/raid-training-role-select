using System.Text;
using Spectre.Console;
using static Role;

partial class Program {
    static Receipt SelectRoles(Form form, bool quiet = false) {
        var squadBuilder = new Squad.Builder();
        var remainingApplicants = CollectApplicants(form);

        remainingApplicants = PlaceTrainers(remainingApplicants, ref squadBuilder, quiet);

        remainingApplicants = PlaceGuaranteeds(remainingApplicants, ref squadBuilder, quiet);

        remainingApplicants = FillSquad(remainingApplicants, ref squadBuilder, quiet);

        remainingApplicants = DesignateNextGuaranteeds(remainingApplicants);

        var squad = squadBuilder.Build();

        if (!quiet) {
            AnsiConsole.WriteLine(squad.ToString());
            if (remainingApplicants.Count > 0) {
                AnsiConsole.WriteLine($"\n## Guaranteed for next week\n{string.Join('\n', remainingApplicants.Select(a => a.RenderedId))}");
            }
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[gray]Seed: {Seed:X8}[/]");
            AnsiConsole.MarkupLine("[gray]Review the source at https://github.com/Streus/raid-training-role-select[/]");
        }

        return new Receipt() {
            Seed = Seed.ToString("X8"),
            Form = form,
            Members = new Receipt.Member[][] {
                squad.Sub1.Select(m => new Receipt.Member(m.Applicant?.DiscordId ?? "", m.AssignedRole.GetPrettyName())).ToArray(),
                squad.Sub2.Select(m => new Receipt.Member(m.Applicant?.DiscordId ?? "", m.AssignedRole.GetPrettyName())).ToArray()
            },
            Guaranteeds = remainingApplicants.Select(a => a.DiscordId).ToArray()
        };
    }

    private static HashSet<Applicant> PlaceTrainers(HashSet<Applicant> remainingApplicants, ref Squad.Builder squad, bool quiet) {
        var trainerApplicants = remainingApplicants
            .Where(a => a.IsTrainer)
            .OrderBy(_ => Random.Next());
        
        var print = new StringBuilder();

        if (trainerApplicants.Any()) {
            foreach (var a in trainerApplicants) {
                print.AppendLine($"Assigned {a.RenderedId} to {a.PrimaryRole.GetPrettyName()}");

                squad.Add(new Member(a, a.PrimaryRole));
            }

            if (!quiet) {
                WriteBox(
                    header: $"[magenta]Assigning {trainerApplicants.Count()} trainers[/]",
                    content: print.ToString()
                );
            }
        }

        return remainingApplicants
            .Except(trainerApplicants)
            .ToHashSet();
    }

    private static HashSet<Applicant> PlaceGuaranteeds(HashSet<Applicant> remainingApplicants, ref Squad.Builder squad, bool quiet) {
        var guaranteedApplicants = remainingApplicants
            .Where(a => a.IsGuaranteed)
            .OrderBy(_ => Random.Next());

        var print = new StringBuilder();

        if (guaranteedApplicants.Any()) {
            foreach (var a in guaranteedApplicants) {
                print.AppendLine($"Assigned {a.RenderedId} to {a.PrimaryRole.GetPrettyName()}");

                squad.Add(new Member(a, a.PrimaryRole));
            }

            if (!quiet) {
                WriteBox(
                    header: $"[magenta]Assigning {guaranteedApplicants.Count()} guaranteeds[/]",
                    content: print.ToString()
                );
            }
        }

        return remainingApplicants
            .Except(guaranteedApplicants)
            .ToHashSet();
    }

    private static HashSet<Applicant> FillSquad(HashSet<Applicant> remainingApplicants, ref Squad.Builder squad, bool quiet) {
        HashSet<Applicant> leftover = remainingApplicants;
        foreach (var (role, count) in squad.GetMissingRoles()) {
            var print = new StringBuilder();

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
                print.AppendLine($"[gray]In drawing:\n{string.Join('\n', prospectives.Select(p => p.RenderedId))}[/]\n");
            }

            foreach (var confirmed in prospectives.Take(count)) {
                var assignedRole = confirmed.PrimaryRole != NONE && role.HasFlag(confirmed.PrimaryRole)
                    ? confirmed.PrimaryRole
                    : role.Pin(Random);
                
                print.AppendLine($"Assigned {confirmed.RenderedId} to {assignedRole.GetPrettyName()}");

                squad.Add(new Member(confirmed, assignedRole));

                leftover = leftover
                    .Except(new Applicant[] { confirmed })
                    .ToHashSet();
            }
            
            int numMissing = Math.Max(0, count - prospectives.Count);
            for (int i = 0; i < numMissing; i++) {
                squad.Add(new Member(role));
            }

            if (!quiet) {
                WriteBox(
                    header: $"[yellow]Assigning {count} {role.GetPrettyName()}[/]",
                    content: print.ToString()
                );
            }
        }
        
        return leftover;
    }

    private static HashSet<Applicant> DesignateNextGuaranteeds(HashSet<Applicant> remainingApplicants) {
        return remainingApplicants
            .Where(a => a.PrimaryRole != NONE)
            .ToHashSet();
    }

    private static void WriteBox(string header, string content) {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(header);
        AnsiConsole.WriteLine(string.Empty.PadRight(50, '─'));
        AnsiConsole.MarkupLine(content);
    }
}
