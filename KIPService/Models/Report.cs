namespace KIPService.Models
{
    public class Report
    {
        public Guid ReportID { get; set; }
        public Guid UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InitDate { get; set; }
        public int SignInCount { get; set; }

    }
}
