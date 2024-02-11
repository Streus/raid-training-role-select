class Member {
    public Applicant Applicant { get; set; } = new Applicant();
    public Role AssignedRole { get; set; } = Role.NONE;

    public string Name => Applicant.RenderedId;

    public override string ToString() => $"{AssignedRole.GetPrettyName()} -> {Name}";
}
