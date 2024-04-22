namespace MyBankAccount.Domain.Exceptions
{
    [Serializable]
    public class BankAccountNotFoundException : Exception
    {
        public BankAccountNotFoundException()
        {
        }

        public BankAccountNotFoundException(string? message) : base(message)
        {
        }

        public BankAccountNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}