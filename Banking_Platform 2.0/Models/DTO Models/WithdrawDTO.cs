namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class WithdrwalDTO
    {
        public int TransactionId { get; set; }
        public Nullable<long> WidtdrawAccountNo { get; set; }
        public string AccountHolderName { get; set; }
        public decimal Amount { get; set; }
        public string WithdrawMode { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public string Remark { get; set; }
    }
}
