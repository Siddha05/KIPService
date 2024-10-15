namespace KIPService.Models
{
    public class DateTimeProvider
    {
        public static DateTime Now => DateTimeProviderContext.Current is null ? DateTime.Now : DateTimeProviderContext.Current.ContextDateTimeNow;
    }
}
