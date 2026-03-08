using System;
using System.Collections.Generic;

namespace Banking_Platform_2._0.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public bool Status { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? BranchId { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual User User { get; set; } = null!;
}
