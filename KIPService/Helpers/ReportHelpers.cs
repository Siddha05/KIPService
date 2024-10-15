using KIPService.Models;

namespace KIPService.Helpers
{
    public static class ReportHelpers
    {
        public static ReportDto AsDTO(this Report report, int process_time) 
        {
            ReportDto? dto = default;
            if (report.IsProcessing(process_time, out var percent))
            {
                dto = new ReportDto
                {
                    Result = null,
                    Percent = percent,
                    Query = report.ReportID.ToString()
                };
            }
            else
            {
                dto = new ReportDto
                {
                    Result = new ReportDtoResult { CountSignIn = report.SignInCount, UserID = report.UserID },
                    Percent = 100,
                    Query = report.ReportID.ToString()
                };
            }
            return dto;
        }
    }
}
