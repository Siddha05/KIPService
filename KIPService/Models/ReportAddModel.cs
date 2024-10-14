namespace KIPService.Models
{
    public class ReportAddModel
    {
        public string UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string ToString() => $"{UserID} {StartDate} - {EndDate}";
    }
}
