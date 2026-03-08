using System;
using System.Collections.Generic;

namespace Banking_Platform_2._0.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public long? FromAcId { get; set; }

    public long? ToAcId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public virtual AccountMst? FromAc { get; set; }

    public virtual AccountMst? ToAc { get; set; }
}
