using MyBankAccount.Core.Models;
using MyBankAccount.PersistanceAdapter.Helpers;
namespace MyBankAccount.PersistanceAdapter
{
    public static class Mappers
    {
        public static IEnumerable<BankAccount> ToDomainBankAccount(this IEnumerable<Persisters.BankAccount> accounts)
        {
            return accounts.Select(c => new BankAccount
            {
                Id = int.Parse(c.Id),
                Balance = int.Parse(c.Balance),
                CreationDate = c.CreationDate.StringToDateFormat(),
                LastOperationDate = c.LastOperationDate.StringToDateFormat(),
            }).ToList();
        }

        public static IEnumerable<Transaction> ToDomainTransaction(this IEnumerable<Persisters.Transaction> transactions)
        {
            return transactions.Select(c => new Transaction
            {
                BankAccountId = int.Parse(c.BankaccountId),
                OperationType = c.OperationType,
                Amount = int.Parse(c.Amount),
                OperationDate = c.OperationDate.StringToDateFormat(),
            }).ToList();
        }
    }
}