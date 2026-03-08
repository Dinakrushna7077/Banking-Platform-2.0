using System;
using System.Collections.Generic;

namespace Banking_Platform_2._0.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public DateTime? Dob { get; set; }

    public string? Gender { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public DateOnly? Doo { get; set; }

    public int? Role { get; set; }

    public string? Aadhar { get; set; }

    public string? Pancard { get; set; }

    public int UserId { get; set; }

    public long? AccId { get; set; }

    public virtual AccountMst? Acc { get; set; }

    public virtual User User { get; set; } = null!;
}
