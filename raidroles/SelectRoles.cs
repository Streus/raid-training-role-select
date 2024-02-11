using static Applicant;
using static Role;

partial class Program {
    static bool FilterForBoonDps(Applicant a, Role boonRole) => 
        (a.PrimaryRole.IsBoonDps() || a.BackupRoles.IsBoonDps()) 
        && (a.PrimaryRole == boonRole || a.BackupRoles.HasFlag(boonRole));

    static void SelectRoles(HashSet<Applicant> applicants) {
        var sub1 = new HashSet<Member>(5);
        var sub2 = new HashSet<Member>(5);
        var remainingApplicants = applicants;

        // ----- Healers -----
        var healerApplicants = remainingApplicants
            .Where(a => a.PrimaryRole.IsHeal() || a.BackupRoles.IsHeal())
            .OrderByDescending(a => Random.Next(99) + a.Bias + (a.PrimaryRole.IsHeal()? PRIMARY_ROLE_BIAS : 0))
            .ToArray();

        Console.WriteLine($"\n----- Selecting 2 Healers from {healerApplicants.Length} -----\n");
        Console.WriteLine($"{string.Join('\n', healerApplicants.Select(a => a.RenderedId))}\n");

        var sub1Healer = new Member { Applicant = healerApplicants[0], AssignedRole = healerApplicants[0].PrimaryRole };
        sub1.Add(sub1Healer);
        remainingApplicants = remainingApplicants
            .Except(new Applicant[]{ sub1Healer.Applicant })
            .ToHashSet();

        var sub2Healer = new Member { Applicant = healerApplicants[1], AssignedRole = healerApplicants[1].PrimaryRole };
        sub2.Add(sub2Healer);
        remainingApplicants = remainingApplicants
            .Except(new Applicant[]{ sub2Healer.Applicant })
            .ToHashSet();

        // ----- Sub 1 Boon DPS -----
        var sub1Boon = sub1Healer.AssignedRole.GetOppositeBoon();
        var sub1BoonApplicants = remainingApplicants
            .Where(a => FilterForBoonDps(a, sub1Boon))
            .OrderByDescending(a => Random.Next(99) + a.Bias + (a.PrimaryRole.IsBoonDps()? PRIMARY_ROLE_BIAS : 0))
            .ToArray();

        Console.WriteLine($"\n----- Selecting 1 {sub1Boon.GetPrettyName()} from {sub1BoonApplicants.Length} -----\n");
        Console.WriteLine($"{string.Join('\n', sub1BoonApplicants.Select(a => a.RenderedId))}\n");

        var sub1Booner = new Member { Applicant = sub1BoonApplicants[0], AssignedRole = sub1Boon };
        sub1.Add(sub1Booner);
        remainingApplicants = remainingApplicants
            .Except(new Applicant[]{ sub1Booner.Applicant })
            .ToHashSet();
        
        // ----- Sub 2 Boon DPS -----
        var sub2Boon = sub2Healer.AssignedRole.GetOppositeBoon();
        var sub2BoonApplicants = remainingApplicants
            .Where(a => FilterForBoonDps(a, sub2Boon))
            .OrderByDescending(a => Random.Next(99) + a.Bias + (a.PrimaryRole.IsBoonDps()? PRIMARY_ROLE_BIAS : 0))
            .ToArray();

        Console.WriteLine($"\n----- Selecting 1 {sub2Boon.GetPrettyName()} from {sub2BoonApplicants.Length} -----\n");
        Console.WriteLine($"{string.Join('\n', sub2BoonApplicants.Select(a => a.RenderedId))}\n");

        var sub2Booner = new Member { Applicant = sub2BoonApplicants[0], AssignedRole = sub2Boon };
        sub2.Add(sub2Booner);
        remainingApplicants = remainingApplicants
            .Except(new Applicant[]{ sub2Booner.Applicant })
            .ToHashSet();

        // ----- x3 Sub 1 DPS -----
        var sub1DpsApplicants = remainingApplicants
            .Where(a => a.PrimaryRole == DPS || a.BackupRoles.HasFlag(DPS))
            .OrderByDescending(a => Random.Next(99) + a.Bias + (a.PrimaryRole == DPS? PRIMARY_ROLE_BIAS : 0))
            .ToArray();

        Console.WriteLine($"\n----- Selecting 3 DPS from {sub1DpsApplicants.Length} -----\n");
        Console.WriteLine($"{string.Join('\n', sub1DpsApplicants.Select(a => a.RenderedId))}\n");

        for (var i = 0; i < 3; i++) {
            var member = new Member { Applicant = sub1DpsApplicants[i], AssignedRole = DPS };
            sub1.Add(member);
            remainingApplicants = remainingApplicants
                .Except(new Applicant[]{ member.Applicant })
                .ToHashSet();
        }

        // ----- x3 Sub 2 DPS -----
        var sub2DpsApplicants = remainingApplicants
            .Where(a => a.PrimaryRole == DPS || a.BackupRoles.HasFlag(DPS))
            .OrderByDescending(a => Random.Next(99) + a.Bias + (a.PrimaryRole == DPS? PRIMARY_ROLE_BIAS : 0))
            .ToArray();

        Console.WriteLine($"\n----- Selecting 3 DPS from {sub2DpsApplicants.Length} -----\n");
        Console.WriteLine($"{string.Join('\n', sub2DpsApplicants.Select(a => a.RenderedId))}\n");

        for (var i = 0; i < 3; i++) {
            var member = new Member { Applicant = sub2DpsApplicants[i], AssignedRole = DPS };
            sub2.Add(member);
            remainingApplicants = remainingApplicants
                .Except(new Applicant[]{ member.Applicant })
                .ToHashSet();
        }

        Console.WriteLine($"\n## Sub 1:\n{string.Join('\n', sub1)}");
        Console.WriteLine($"\n## Sub 2:\n{string.Join('\n', sub2)}");
        Console.WriteLine($"\n## Guaranteed for next week:\n{string.Join('\n', remainingApplicants.Select(a => a.RenderedId))}");
        Console.WriteLine($"\n*Seed: {Seed:X8}*");
        Console.WriteLine("*Review the source at https://github.com/Streus/raid-training-role-select*");
    }
}
