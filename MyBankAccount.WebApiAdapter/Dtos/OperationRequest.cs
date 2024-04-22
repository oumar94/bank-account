namespace MyBankAccount.WebApiAdapter.Dtos
{
    public class OperationRequest
    {
        public int BankAccountId { get; set; }
        public int Amount { get; set; }
    }
}