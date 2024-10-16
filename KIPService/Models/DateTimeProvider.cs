namespace KIPService.Models
{
    /// <summary>
    /// DateTime provider for test
    /// </summary>
    public class DateTimeProvider
    {
        /// <summary>
        /// Текущее дата и время
        /// </summary>
        public static DateTime Now => DateTimeProviderContext.Current is null ? DateTime.Now : DateTimeProviderContext.Current.ContextDateTimeNow;
    }
}
