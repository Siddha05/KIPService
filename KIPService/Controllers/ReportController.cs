using KIPService.DbContexts;
using KIPService.Helpers;
using KIPService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace KIPService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        AppDbContext _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id">Идентификатор пользователя</param>
        /// <param name="from">Начало периода</param>
        /// <param name="to">Окончание периода</param>
        [HttpPost("user_statistics")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(200)]
        public async Task<IResult> SaveStatistic([FromBody]ReportAddModel model, ILogger<ReportController> logger)
        {
            logger.LogInformation($"Invoke SaveStatistic: {model}");
            var report = new Report(model.UserID, model.StartDate, model.EndDate);
            await _db.Reports.AddAsync(report);
            await _db.SaveChangesAsync();
            return Results.Ok(report.ReportID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report_id">Guid запроса</param>
        /// <returns></returns>
        [HttpGet("info")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IResult> GetStatistic([FromQuery]string report_id, IConfiguration configuration, ILogger<ReportController> logger)
        {
            logger.LogInformation($"Invoke GetStatistic {report_id}");
            int processingTime = configuration.GetValue<int>("ReportExecutionTime");
            processingTime = processingTime is 0 ? 60 : processingTime;
            if (Guid.TryParse(report_id, out var guid))
            {
                var report = _db.Reports.FirstOrDefault(s => s.ReportID == guid);
                if (report is null)
                {
                    return Results.StatusCode(StatusCodes.Status204NoContent);
                }
                else
                {
                    return Results.Ok(report.AsDTO(processingTime));
                }
            }
            else
            {
                return Results.BadRequest($"Incorrect giud {report_id}");
            }
        }

        public ReportController(AppDbContext db) => _db = db;
    }
}
