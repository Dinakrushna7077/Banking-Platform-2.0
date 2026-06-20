using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_Platform_2._0.Models.DTO_Models
{
    public class LastTransactionDTO
    {
        public int TransactionId { get; set; }

        public long? FromAcId { get; set; }

        public long? ToAcId { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; } = null!;

        public DateTime? Timestamp { get; set; }
    }
}
