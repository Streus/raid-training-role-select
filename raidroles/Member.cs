class Member {
    public Member(Applicant a, Role assignedRole) {
        Applicant = a;
        AssignedRole = assignedRole;
    }

    public Member(Role assignedRole) {
        Applicant = null;
        AssignedRole = assignedRole;
    }

    public Applicant? Applicant { get; }
    public Role AssignedRole { get; }

    public string Name => Applicant?.RenderedId ?? "OPEN";

    public override string ToString() => $"{AssignedRole.GetPrettyName()} -> {Name}";
}
