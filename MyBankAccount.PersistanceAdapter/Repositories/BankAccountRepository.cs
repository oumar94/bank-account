using MyBankAccount.Core.Interfaces;
using MyBankAccount.Core.Models;
using MyBankAccount.Domain.Enums;
using MyBankAccount.PersistanceAdapter.Helpers;

namespace MyBankAccount.PersistanceAdapter.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly ICsvFileWriter _writer;
        public BankAccountRepository(ICsvFileWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
        public async Task<BankAccount> GetCurrentBalanceAsync(int bankAccountId)
        {
            var result = CsvFileHelper.ReadCsvFile<Persisters.BankAccount>(Constants.BankAccountFilePath);
            return result.ToDomainBankAccount().Where(c => c.Id == bankAccountId).FirstOrDefault();
        }

        public async Task<IEnumerable<Transaction>> GetPreviousTransactionsAsync(int bankAccountId)
        {
            var result = CsvFileHelper.ReadCsvFile<Persisters.Transaction>(Constants.TransactionsFilePath);
            return result.ToDomainTransaction().Where(c => c.BankAccountId == bankAccountId);
        }

        public async Task<bool> DepositMoneyAsync(int newBalance, int bankAccountId)
        {
            return await _writer.UpdateCsvFileBankAccountAsync(Constants.BankAccountFilePath, newBalance, bankAccountId, OperationType.Deposit.ToString());
        }

        public async Task<bool> WithdrawMoneyAsync(int newBalance, int bankAccountId)
        {
            return await _writer.UpdateCsvFileBankAccountAsync(Constants.BankAccountFilePath, newBalance, bankAccountId, OperationType.Withdraw.ToString());
        }
    }
}