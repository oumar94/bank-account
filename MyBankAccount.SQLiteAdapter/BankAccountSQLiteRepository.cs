using Microsoft.EntityFrameworkCore;
using MyBankAccount.Core.Interfaces;
using MyBankAccount.Core.Models;
using MyBankAccount.Domain.Enums;

namespace MyBankAccount.SQLiteAdapter
{
    public class BankAccountSQLiteRepository : IBankAccountRepository
    {
        private readonly BankAccountDBContext _context;
        public BankAccountSQLiteRepository(BankAccountDBContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetCurrentBalanceAsync(int bankAccountId)
        {
            return _context.BankAccounts.FirstOrDefault(x => x.Id == bankAccountId);
        }

        public async Task<IEnumerable<Transaction>> GetPreviousTransactionsAsync(int bankAccountId)
        {
            return await _context.Transactions.Where(x => x.BankAccountId == bankAccountId).ToListAsync();
        }

        public async Task<bool> DepositMoneyAsync(int newBalance, int bankAccountId)
        {
            var existing = _context.BankAccounts.FirstOrDefault(x => x.Id == bankAccountId);
            if (existing == null) return false;

            await CreatebankAccountTransactionsLogAsync(newBalance, existing.Balance, bankAccountId, OperationType.Deposit.ToString());
            existing.Balance = newBalance;
            existing.LastOperationDate = DateTime.Now;
            _context.BankAccounts.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> WithdrawMoneyAsync(int newBalance, int bankAccountId)
        {
            var existing = _context.BankAccounts.FirstOrDefault(x => x.Id == bankAccountId);
            if (existing == null) return false;
            await CreatebankAccountTransactionsLogAsync(newBalance, existing.Balance, bankAccountId, OperationType.Withdraw.ToString());

            existing.Balance = newBalance;
            existing.LastOperationDate = DateTime.Now;
            _context.BankAccounts.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task CreatebankAccountTransactionsLogAsync(int newBalance, int oldbalance, int bankAccountId, string operationType)
        {
            var transcationLog = new Transaction
            {
                BankAccountId = bankAccountId,
                Amount = Math.Abs(newBalance - oldbalance),
                OperationDate = DateTime.Now,
                OperationType = operationType
            };
            _context.Transactions.Add(transcationLog);
            await _context.SaveChangesAsync();
        }
    }
}