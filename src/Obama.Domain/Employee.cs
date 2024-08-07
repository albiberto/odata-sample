﻿namespace Obama.Domain;

public record Employee(string GivenName, string FamilyName, string Mail, Guid RoleId) : Entity
{
    public Employee() : this(string.Empty, string.Empty, string.Empty, Guid.Empty)
    {
    }
    
    public string GivenName { get; set; } = GivenName;
    public string FamilyName { get; set; } = FamilyName;
    public string Mail { get; set; } = Mail;

    public Guid RoleId { get; set; } = RoleId;
    public Role Role { get; private set; }
}