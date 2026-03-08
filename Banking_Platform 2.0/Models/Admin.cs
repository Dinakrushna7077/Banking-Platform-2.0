using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Banking_Platform_2._0.Models;

[Table("Admin")]
public partial class Admin
{
    [Key]
    [Column("AdminID")]
    public int AdminId { get; set; }

    public int UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    public bool Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    public int? BranchId { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Admins")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Admins")]
    public virtual User User { get; set; } = null!;
}
