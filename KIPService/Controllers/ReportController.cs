using KIPService.DbContexts;
using KIPService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="report_giud">Guid запроса</param>
        /// <returns></returns>
        [HttpGet("info")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IResult> GetStatistic([FromQuery]string report_giud, IConfiguration configuration, ILogger<ReportController> logger)
        {
            logger.LogInformation($"Invoke GetStatistic {report_giud}");
            int executionTime = configuration.GetValue<int>("ReportExecutionTime");
            executionTime = executionTime is 0 ? 60 : executionTime;
            logger.LogWarning($"{executionTime}");
            return Results.Ok(report_giud);
        }

        public ReportController(AppDbContext db) => _db = db;
    }
}
