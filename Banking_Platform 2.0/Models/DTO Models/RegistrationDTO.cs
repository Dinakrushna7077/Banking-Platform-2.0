namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class RegistrationDTO
    {
        // Step 1
        public long AccountNumber { get; set; }
        public string IFSC { get; set; }

        // Fetched after verification
        public string FullName { get; set; }
        public string DOB { get; set; }

        // Step 2
        public string Email { get; set; }
        public string Phone { get; set; }

        // Step 3
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
