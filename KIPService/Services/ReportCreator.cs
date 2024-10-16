
using KIPService.DbContexts;

namespace KIPService.Services
{
    /// <summary>
    /// Имитируем выполнение отчета (запроса). Выполненым считаем тот, у которого поле count_sign_in не null
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public class ReportCreator (IServiceProvider serviceProvider, ILogger<ReportCreator> logger, IConfiguration configuration) : BackgroundService
    {
        private readonly int _scan_period = 5000;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Start ReportCreator service");
            await Task.Delay( 5000, stoppingToken );
            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                if (db == null)
                {
                    logger.LogWarning($"Not found {nameof(AppDbContext)}");
                    await Task.Delay(_scan_period, stoppingToken);
                    continue;

                }
                var process_time = configuration.GetValue<int>("ReportExecutionTime");
                var reports = db.Reports.Where(w => (DateTime.Now - w.InitDate).TotalSeconds >= process_time && w.SignInCount == null);
                logger.LogWarning($"Found: {reports.Count()}");
                foreach (var report in reports)
                {
                    report.SignInCount = Random.Shared.Next(0, 100);
                }
                await db.SaveChangesAsync();
                await Task.Delay(_scan_period, stoppingToken);
            }
        }
    }
}
