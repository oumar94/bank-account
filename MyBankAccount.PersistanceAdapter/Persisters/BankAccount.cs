using CsvHelper.Configuration.Attributes;

namespace MyBankAccount.PersistanceAdapter.Persisters
{
    [Serializable]
    public class BankAccount
    {
        [Name("id"), Index(1)]
        public string Id { get; set; }

        [Name("balance"), Index(2)]
        public string Balance { get; set; }

        [Name("creation-date"), Index(3)]
        public string CreationDate { get; set; }

        [Name("last-operation-date"), Index(4)]
        public string LastOperationDate { get; set; }
    }
}