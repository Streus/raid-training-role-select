partial class Program {
    static HashSet<Applicant> CollectApplicants(Form form) {
        var allApplicants = new HashSet<Applicant>();

        // Primary Roles
        foreach (var name in form.PrimaryRoleApplicants.QHeal) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.PrimaryRole = Role.Q_HEAL;
        }
        foreach (var name in form.PrimaryRoleApplicants.AHeal) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.PrimaryRole = Role.A_HEAL;
        }
        foreach (var name in form.PrimaryRoleApplicants.QDps) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.PrimaryRole = Role.Q_DPS;
        }
        foreach (var name in form.PrimaryRoleApplicants.ADps) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.PrimaryRole = Role.A_DPS;
        }
        foreach (var name in form.PrimaryRoleApplicants.Dps) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.PrimaryRole = Role.DPS;
        }

        // Backup Roles
        foreach (var name in form.BackupRoleApplicants.BkHeal) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.BackupRoles |= Role.Q_HEAL | Role.A_HEAL;
        }
        foreach (var name in form.BackupRoleApplicants.BkBoon) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.BackupRoles |= Role.Q_DPS | Role.A_DPS;
        }
        foreach (var name in form.BackupRoleApplicants.BkDps) {
            var applicant = GetOrCreate(name, allApplicants);
            applicant.BackupRoles |= Role.DPS;
        }

        // Trainers
        foreach (var trainer in form.Trainers) {
            var applicant = GetOrCreate(trainer, allApplicants);
            applicant.IsTrainer = true;
        }

        // Guaranteed
        foreach (var trainer in form.Guaranteed) {
            var applicant = GetOrCreate(trainer, allApplicants);
            applicant.IsGuaranteed = true;
        }

        return allApplicants;
    }

    static Applicant GetOrCreate(string applicant, HashSet<Applicant> allApplicants) {
        var potentialApplicant = new Applicant { 
            DiscordId = applicant,
            PrimaryRole = Role.NONE
        };
        if (allApplicants.TryGetValue(potentialApplicant, out Applicant? existing)) {
            return existing;
        } else {
            allApplicants.Add(potentialApplicant);
            return potentialApplicant;
        }
    }
}
