using KIPService.DbContexts;
using KIPService.Helpers;
using KIPService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace KIPService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController(AppDbContext db) : ControllerBase
    {
        /// <summary>
        /// Добавляет запрос в БД
        /// </summary>
        /// <response code="200">Успешно</response>
        /// <response code="503">Ошибка сервера</response>
        /// <response code="400">Ошибка API</response>
        [HttpPost("user_statistics")]
        public async Task<IResult> SaveStatistic([FromBody]ReportAddModel model, ILogger<ReportController> logger)
        {
            logger.LogInformation($"Invoke SaveStatistic: {model}");
            var report = new Report(model.UserID, model.StartDate, model.EndDate);
            await db.Reports.AddAsync(report);
            await db.SaveChangesAsync();
            return Results.Ok(report.ReportID);
        }

        /// <summary>
        /// Получить запрос из БД
        /// </summary>
        /// <param name="report_id">Guid запроса</param>
        /// <returns></returns>
        /// <response code="200">Успешно</response>
        /// <response code="204">Данных нет</response>
        /// <response code="503">Ошибка сервера</response>
        /// <response code="400">Ошибка API</response>
        [HttpGet("info")]
        public async Task<IResult> GetStatistic([FromQuery]string report_id, IConfiguration configuration, ILogger<ReportController> logger)
        {
            logger.LogInformation($"Invoke GetStatistic {report_id}");
            int processingTime = configuration.GetValue<int>("ReportExecutionTime");
            processingTime = processingTime is 0 ? 60 : processingTime;
            if (Guid.TryParse(report_id, out var guid))
            {
                var report = await db.Reports.FirstOrDefaultAsync(s => s.ReportID == guid);
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
    }
}
