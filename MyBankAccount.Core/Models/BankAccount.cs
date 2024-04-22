namespace MyBankAccount.Core.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastOperationDate { get; set; }
    }
}