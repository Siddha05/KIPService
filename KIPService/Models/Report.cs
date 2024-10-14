using System.ComponentModel.DataAnnotations;

namespace KIPService.Models
{
    public class Report
    {
        [Key]
        public Guid ReportID { get; set; }
        public string UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InitDate { get; set; }
        public int SignInCount { get; set; }

        public Report(string userID, DateTime from, DateTime to)
        {
            UserID = userID;
            StartDate = from;
            EndDate = to;
            InitDate = DateTime.Now;
        }
        public Report()
        {
            
        }

    }
}
