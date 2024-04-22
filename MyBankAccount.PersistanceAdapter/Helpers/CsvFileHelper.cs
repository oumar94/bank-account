using CsvHelper;
using System.Globalization;

namespace MyBankAccount.PersistanceAdapter.Helpers
{
    public static class CsvFileHelper
    {
        public static IEnumerable<T> ReadCsvFile<T>(string filename)
        {
            var entities = new List<T>();
            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                entities = csv.GetRecords<T>().ToList();
            }
            return entities;
        }
    }
}