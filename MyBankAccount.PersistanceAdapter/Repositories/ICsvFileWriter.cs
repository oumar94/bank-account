namespace MyBankAccount.PersistanceAdapter.Repositories
{
    public interface ICsvFileWriter
    {
        Task<bool> UpdateCsvFileBankAccountAsync(string filename, int newBalance, int bankAcountId, string operationType);
    }
}