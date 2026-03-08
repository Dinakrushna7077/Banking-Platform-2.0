using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Banking_Platform_2._0.Models;

[Table("Branch")]
[Index("BranchCode", Name = "UQ__Branch__1C61B8882A844E29", IsUnique = true)]
[Index("Ifsccode", Name = "UQ__Branch__6C74377FFD45E809", IsUnique = true)]
public partial class Branch
{
    [Key]
    public int BranchId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string BranchCode { get; set; } = null!;

    [Column("IFSCCode")]
    [StringLength(20)]
    [Unicode(false)]
    public string Ifsccode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string State { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<AccountMst> AccountMsts { get; set; } = new List<AccountMst>();

    [InverseProperty("Branch")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
}
