using CsvHelper.Configuration.Attributes;

namespace MyBankAccount.PersistanceAdapter.Persisters
{
    [Serializable]
    public class Transaction
    {
        [Name("bankaccount-id"), Index(1)]
        public string BankaccountId { get; set; }

        [Name("operation-type"), Index(2)]
        public string OperationType { get; set; }

        [Name("amount"), Index(3)]
        public string Amount { get; set; }

        [Name("operation-date"), Index(4)]
        public string OperationDate { get; set; }
    }
}