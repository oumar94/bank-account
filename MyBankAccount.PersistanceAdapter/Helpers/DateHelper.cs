namespace MyBankAccount.PersistanceAdapter.Helpers
{
    public static class DateHelper
    {
        public static string FormatDate(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static DateTime StringToDateFormat(this string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime parsedDate))
            {
                return parsedDate;
            }
            throw new FormatException();
        }
    }
}