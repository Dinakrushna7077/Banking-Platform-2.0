using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Banking_Platform_2._0.Models;

[Table("Customer")]
public partial class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [Column("DOB", TypeName = "datetime")]
    public DateTime? Dob { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Gender { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? FatherName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? MotherName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? City { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Country { get; set; }

    [Column("DOO")]
    public DateOnly? Doo { get; set; }

    public int? Role { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string? Aadhar { get; set; }

    [Column("PANCard")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Pancard { get; set; }

    public int UserId { get; set; }

    public long? AccId { get; set; }

    [ForeignKey("AccId")]
    [InverseProperty("Customers")]
    public virtual AccountMst? Acc { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Customers")]
    public virtual User User { get; set; } = null!;
}
