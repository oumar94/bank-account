using MyBankAccount.Core.Interfaces;
using MyBankAccount.Core.Models;
using MyBankAccount.Domain.Exceptions;

namespace MyBankAccount.Domain.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _repository;
        public BankAccountService(IBankAccountRepository rd)
        {
            _repository = rd;
        }
        public async Task<bool> DepositMoneyAsync(int amount, int bankAccountId)
        {
            var account = await GetCurrentBalanceAsync(bankAccountId);
            if (account == null) throw new BankAccountNotFoundException(Constants.BankAccountNotFoundMessage);
            var newBalance = account.Balance + amount;
            return await _repository.DepositMoneyAsync(newBalance, bankAccountId);
        }
        public async Task<bool> WithDrawMoneyAsync(int amount, int bankAccountId)
        {
            var account = await GetCurrentBalanceAsync(bankAccountId);
            if (account == null) throw new BankAccountNotFoundException(Constants.BankAccountNotFoundMessage);
            if (amount > account.Balance) throw new InsufficientBalanceException(Constants.InsufficientBalanceMessage);
            var newBalance = account.Balance - amount;
            return await _repository.WithdrawMoneyAsync(newBalance, bankAccountId);
        }

        public async Task<BankAccount> GetCurrentBalanceAsync(int bankAccountId)
        {
            return await _repository.GetCurrentBalanceAsync(bankAccountId);
        }

        public async Task<IEnumerable<Transaction>> GetPreviousTransactionsAsync(int bankAccountId)
        {
            return await _repository.GetPreviousTransactionsAsync(bankAccountId);
        }
    }
}