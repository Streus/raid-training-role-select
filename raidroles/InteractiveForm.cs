partial class Program {
    static Form HandleInteractiveForm() {
        var form = new Form();

        Console.WriteLine("Specify trainers (empty to end):");
        form.Trainers = CollectList();

        Console.WriteLine("Specify guaranteeds (empty to end):");
        form.Guaranteed = CollectList();

        Console.WriteLine("Quick Heal applicants (empty to end):");
        form.PrimaryRoleApplicants.QHeal = CollectList();

        Console.WriteLine("Alac Heal applicants (empty to end):");
        form.PrimaryRoleApplicants.AHeal = CollectList();

        Console.WriteLine("Quick DPS applicants (empty to end):");
        form.PrimaryRoleApplicants.QDps = CollectList();

        Console.WriteLine("Alac DPS applicants (empty to end):");
        form.PrimaryRoleApplicants.ADps = CollectList();

        Console.WriteLine("DPS applicants (empty to end):");
        form.PrimaryRoleApplicants.Dps = CollectList();

        Console.WriteLine("Specify backup healers (empty to end):");
        form.BackupRoleApplicants.BkHeal = CollectList();

        Console.WriteLine("Specify backup booners (empty to end):");
        form.BackupRoleApplicants.BkBoon = CollectList();

        Console.WriteLine("Specify backup dpses (empty to end):");
        form.BackupRoleApplicants.BkDps = CollectList();

        Console.WriteLine("Specify tanks (empty to end):");
        form.MechanicsApplicants.Tank = CollectList();

        return form;
    }

    static string[] CollectList() {
        var list = new List<string>();
        while (true) {
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input)) {
                break;
            }
            list.Add(input);
        }
        return list.ToArray();
    }
}