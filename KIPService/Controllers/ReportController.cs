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
    public class ReportController : ControllerBase
    {
        AppDbContext _db;

        /// <summary>
        /// Добавляет запрос в БД
        /// </summary>
        /// <param name="user_id">Идентификатор пользователя</param>
        /// <param name="from">Начало периода</param>
        /// <param name="to">Окончание периода</param>
        /// <response code="200">Успешно</response>
        /// <response code="503">Ошибка сервера</response>
        /// <response code="400">Ошибка API</response>
        [HttpPost("user_statistics")]
        public async Task<IResult> SaveStatistic([FromBody]ReportAddModel model, ILogger<ReportController> logger)
        {
            logger.LogInformation($"Invoke SaveStatistic: {model}");
            var report = new Report(model.UserID, model.StartDate, model.EndDate);
            await _db.Reports.AddAsync(report);
            await _db.SaveChangesAsync();
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
                var report = await _db.Reports.FirstOrDefaultAsync(s => s.ReportID == guid);
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
