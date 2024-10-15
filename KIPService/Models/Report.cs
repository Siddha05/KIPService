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

        /// <summary>
        /// В обработке?
        /// </summary>
        /// <param name="process_time">Время обработки</param>
        /// <returns></returns>
        public bool IsProcessing(int process_time, out double percent)
        {
            var time = (DateTimeProvider.Now - InitDate).TotalSeconds;
            if (time < 0)
            {
                percent = 0;
                return true;
            }
            var ptr = 100 * time / process_time;
            percent = ptr > 100 ? 100 : ptr;
            return time < process_time;
        }
        
        public Report(string userID, DateTime from, DateTime to)
        {
            UserID = userID;
            StartDate = from;
            EndDate = to;
            InitDate = DateTime.Now;
        }
        public Report() { }

    }

    public class ReportDtoResult
    {
        public string UserID { get; set; }
        public int CountSignIn { get; set; }
    }

    public class ReportDto()
    {
        public string? Query { get; set; }
        public double Percent { get; set; }
        public ReportDtoResult? Result { get; set; }
    }
}
