using MyBankAccount.Core.Models;

namespace MyBankAccount.Core.Interfaces
{
    public interface IBankAccountService
    {
        Task<bool> DepositMoneyAsync(int amount, int bankAccountId);
        Task<bool> WithDrawMoneyAsync(int amount, int bankAccountId);
        Task<BankAccount> GetCurrentBalanceAsync(int bankAccountId);
        Task<IEnumerable<Transaction>> GetPreviousTransactionsAsync(int bankAccountId);
    }
}