namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public Nullable<long> FromAcId { get; set; }
        public Nullable<long> ToAcId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
    }
}
