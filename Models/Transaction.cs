namespace ATM_Simulator.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal BalanceAfterTransaction { get; set; }
    }
}