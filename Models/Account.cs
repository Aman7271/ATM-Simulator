namespace ATM_Simulator.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string AccountHolderName { get; set; } = string.Empty;

        public int PIN { get; set; }

        public decimal Balance { get; set; }
    }
}