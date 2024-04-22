using MyBankAccount.Core.Models;

namespace MyBankAccount.Core.Interfaces
{
    public interface IBankAccountRepository
    {
        Task<bool> DepositMoneyAsync(int newBalance, int bankAccountId);
        Task<bool> WithdrawMoneyAsync(int newBalance, int bankAccountId);
        Task<BankAccount> GetCurrentBalanceAsync(int bankAccountId);
        Task<IEnumerable<Transaction>> GetPreviousTransactionsAsync(int bankAccountId);
    }
}