namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class CheckBalanceDTO
    {
        public long AccNo { get; set; }
        public long AccId { get; set; }
        public string CustomerName { get; set; }
        public string AccType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDt { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public long Mobile { get; set; }
        public string Status { get; set; }
        /*public bool Success { get; set; }
        public string Message { get; set; }

        // Account Card
        public string AccountNumber { get; set; }
        public string HolderName { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }

        // Details Panel
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string Status { get; set; }
        public string OpeningDate { get; set; }
        public string Mobile { get; set; }

        // Stats Boxes
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public int TotalTransactions { get; set; }
        public string LastTransactionDate { get; set; }*/
    }
}
