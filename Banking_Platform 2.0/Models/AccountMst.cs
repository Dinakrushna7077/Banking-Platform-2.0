using System;
using System.Collections.Generic;

namespace Banking_Platform_2._0.Models;

public partial class AccountMst
{
    public long AccId { get; set; }

    public int UserId { get; set; }

    public long AccountNo { get; set; }

    public decimal? Balance { get; set; }

    public string AccType { get; set; } = null!;

    public DateOnly CreatedDt { get; set; }

    public int? BranchId { get; set; }

    public bool? Status { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Transaction> TransactionFromAcs { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionToAcs { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
