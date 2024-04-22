using CsvHelper;
using MyBankAccount.PersistanceAdapter.Helpers;
using MyBankAccount.PersistanceAdapter.Persisters;
using System.Globalization;

namespace MyBankAccount.PersistanceAdapter.Repositories
{
    public class CsvFileWriter : ICsvFileWriter
    {
        public async Task<bool> UpdateCsvFileBankAccountAsync(string filename, int newBalance, int bankAcountId, string operationType)
        {
            List<BankAccount> newAccounts = new List<BankAccount>();

            var oldAccounts = CsvFileHelper.ReadCsvFile<BankAccount>(filename);
            foreach (var account in oldAccounts)
            {
                if (account.Id.Equals(bankAcountId.ToString()))
                {
                    var newTransactionLine = CreateTransaction(bankAcountId, operationType, newBalance, account.Balance);

                    account.Balance = newBalance.ToString();
                    account.LastOperationDate = DateTime.Now.FormatDate();
                    await WriteCsvFileTransactionAsync(Constants.TransactionsFilePath, newTransactionLine);
                }
                newAccounts.Add(account);
            }
            return await WriteCsvFileBankAccountAsync(filename, newAccounts);
        }

        private Transaction CreateTransaction(int bankAcountId, string operationType, int newBalance, string oldBalance)
        {
            return new Transaction
            {
                BankaccountId = bankAcountId.ToString(),
                OperationType = operationType,
                Amount = Math.Abs(newBalance - int.Parse(oldBalance)).ToString(),
                OperationDate = DateTime.Now.FormatDate(),
            };
        }

        private async Task<bool> WriteCsvFileBankAccountAsync(string filename, IEnumerable<BankAccount> accounts)
        {
            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(accounts);
            }
            return true;
        }
        private async Task WriteCsvFileTransactionAsync(string filename, Transaction newTransactionLine)
        {
            var transactionLines = CsvFileHelper.ReadCsvFile<Transaction>(filename).ToList();
            transactionLines.Add(newTransactionLine);

            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(transactionLines);
            }
        }
    }
}