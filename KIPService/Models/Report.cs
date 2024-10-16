using System.ComponentModel.DataAnnotations;

namespace KIPService.Models
{
    public class Report
    {
        /// <summary>
        /// Guid отчета
        /// </summary>
        [Key]
        public Guid ReportID { get; set; }
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
        /// <summary>
        /// Начало обработки
        /// </summary>
        public DateTime InitDate { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public int? SignInCount { get; set; }

        /// <summary>
        /// В обработке?
        /// </summary>
        /// <param name="process_time">Время обработки</param>
        /// /// <param name="percent">Процент выполнения</param>
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
#pragma warning disable CS1591
        public Report(string userID, DateTime from, DateTime to)
        {
            UserID = userID;
            StartDate = from;
            EndDate = to;
            InitDate = DateTime.Now;
        }
        public Report() { }
#pragma warning restore 
    }

    public class ReportDtoResult
    {
        public string? UserID { get; set; }
        public int? CountSignIn { get; set; }
    }

    public class ReportDto()
    {
        public string? Query { get; set; }
        public double Percent { get; set; }
        public ReportDtoResult? Result { get; set; }
    }
}
