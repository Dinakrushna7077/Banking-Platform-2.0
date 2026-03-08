using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Banking_Platform_2._0.Models;

public partial class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    public long? FromAcId { get; set; }

    public long? ToAcId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Type { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Timestamp { get; set; }

    [ForeignKey("FromAcId")]
    [InverseProperty("TransactionFromAcs")]
    public virtual AccountMst? FromAc { get; set; }

    [ForeignKey("ToAcId")]
    [InverseProperty("TransactionToAcs")]
    public virtual AccountMst? ToAc { get; set; }
}
