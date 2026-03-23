using System.ComponentModel.DataAnnotations;

namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class ModifyAccountDto
    {
        // ── Read-only (carried via hidden field) ──────────────────────────────
        [Required]
        public string AccountNumber { get; set; }

        public string IFSCCode { get; set; }  // Auto-filled readonly

        // ── Personal Info ─────────────────────────────────────────────────────
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Father name is required.")]
        [StringLength(100)]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Mother name is required.")]
        [StringLength(100)]
        public string MotherName { get; set; }

        // ── Contact Info ──────────────────────────────────────────────────────
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        // ── Address ───────────────────────────────────────────────────────────
        [Required(ErrorMessage = "Please select a state.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please select a country.")]
        public string Country { get; set; }

        // ── Branch & Account Type ─────────────────────────────────────────────
        [Required(ErrorMessage = "Please select a branch.")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Please select an account type.")]
        public string AccountType { get; set; }
    }
}
