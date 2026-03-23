namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class DepositeDTO
    {
        public int TransactionId { get; set; }
        public Nullable<long> DepositeAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string DepositedBy { get; set; }
        public decimal Amount { get; set; }
        public decimal AvailableBalance{ get; set; }
        public string DepositeMode { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public string Remark { get; set; }
    }
}
