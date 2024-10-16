namespace KIPService.Models
{
    public class ReportAddModel
    {
        /// <summary>
        /// Guid пользователя
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// Период с
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Период по
        /// </summary>
        public DateTime EndDate { get; set; }
#pragma warning disable CS1591
        public override string ToString() => $"{UserID} {StartDate} - {EndDate}";
    }
}
