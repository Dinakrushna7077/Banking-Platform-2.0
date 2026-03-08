using System;
using System.Collections.Generic;

namespace Banking_Platform_2._0.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string Ifsccode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AccountMst> AccountMsts { get; set; } = new List<AccountMst>();

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
}
