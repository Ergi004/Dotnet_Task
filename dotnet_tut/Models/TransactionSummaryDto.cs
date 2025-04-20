namespace dotnet_tut.Models
{
 
    public class TransactionSummaryDto
    {
        public int TotalTransactions { get; set; }

        public int TotalCredits { get; set; }

        public int TotalDebits { get; set; }

        public int NetBalance { get; set; }
    }
}
