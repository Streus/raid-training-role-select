class Applicant {
    public const int TRAINER_BIAS = 10000;
    public const int GUARANTEED_BIAS = 1000;
    public const int PRIMARY_ROLE_BIAS = 100;

    public string DiscordId { get; set; } = "0";
    public Role PrimaryRole { get; set; } = Role.NONE;
    public Role BackupRoles { get; set; } = Role.NONE;
    public bool IsGuaranteed { get; set; } = false;
    public bool IsTrainer { get; set; } = false;
    public int Bias => IsTrainer ? TRAINER_BIAS : IsGuaranteed ? GUARANTEED_BIAS : 0;

    public string RenderedId => $"<@{DiscordId}>";

    public override bool Equals(object? obj) {
        if (obj == null) return false;

        if (obj.GetType().Equals(typeof(Applicant))) {
            return DiscordId.Equals(((Applicant)obj).DiscordId);
        }
        return false;
    }

    public override int GetHashCode() => DiscordId.GetHashCode();

    public override string ToString() {
        var baseStr = $"id: {DiscordId}, primary: {PrimaryRole}, backup: {BackupRoles}";
        if (IsTrainer) baseStr = $"[TRAINER] {baseStr}";
        if (IsGuaranteed) baseStr = $"[GUARANTEED] {baseStr}";
        return baseStr;
    }
}
