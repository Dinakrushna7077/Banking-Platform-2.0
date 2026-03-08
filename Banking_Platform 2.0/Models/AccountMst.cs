using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Banking_Platform_2._0.Models;

[Table("Account_mst")]
public partial class AccountMst
{
    [Key]
    public long AccId { get; set; }

    public long AccountNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Balance { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string AccType { get; set; } = null!;

    public DateOnly CreatedDt { get; set; }

    public int? BranchId { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("AccountMsts")]
    public virtual Branch? Branch { get; set; }

    [InverseProperty("Acc")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("FromAc")]
    public virtual ICollection<Transaction> TransactionFromAcs { get; set; } = new List<Transaction>();

    [InverseProperty("ToAc")]
    public virtual ICollection<Transaction> TransactionToAcs { get; set; } = new List<Transaction>();

    [InverseProperty("Acc")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
