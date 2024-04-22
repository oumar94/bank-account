namespace MyBankAccount.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string OperationType { get; set; }
        public DateTime OperationDate { get; set; }
        public int Amount { get; set; }
    }
}