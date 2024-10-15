using KIPService.DbContexts;
using KIPService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KIPService.Controllers.Tests
{
    [TestClass()]
    public class ReportControllerTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            using (var context = new AppDbContext(DbContextOptions!))
            {
                var reports = context.Reports;
                context.RemoveRange(reports);
                context.SaveChanges();
            }
        }

        private static DbContextOptions<AppDbContext>? DbContextOptions { get; set; }
        public TestContext TestContext { get; set; }

        private ILogger<ReportController> CreateLogger() => new Mock<ILogger<ReportController>>().Object;
        private IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ReportExecutionTime", "60" }
            }).Build();
        }

        private IEnumerable<Report> GetTestData()
        {
            yield return new Report
            {
                ReportID = Guid.Parse("8cace8a4-7445-4d21-a5b5-fc330f9b3307"),
                EndDate = DateTime.Now.AddDays(-5),
                StartDate = DateTime.Now.AddDays(-10),
                InitDate = new DateTime(2024, 10, 15, 12, 10, 10),
                UserID = "8cbf468f-dfeb-4ddd-b04c-ccbf47d23de2",
                SignInCount = 2,
            };
            yield return new Report
            {
                ReportID = Guid.Parse("9cace8a4-7445-4d21-a5b5-fc330f9b3307"),
                EndDate = DateTime.Now.AddDays(-2),
                StartDate = DateTime.Now.AddDays(-4),
                InitDate = new DateTime(2024, 10, 15, 12, 7, 30),
                UserID = "9cbf468f-dfeb-4ddd-b04c-ccbf47d23de2",
                SignInCount = 5,
            };
        }

        [TestMethod()]
        public void SaveStatisticTest()
        {
            var model = new ReportAddModel
            {
                UserID = "acbf468f-dfeb-4ddd-b04c-ccbf47d23de2",
                StartDate = DateTime.Now.AddDays(-100),
                EndDate = DateTime.Now.AddDays(-32),
            };
            using (var context = new AppDbContext(DbContextOptions!))
            {
                var cntr = new ReportController(context);
                cntr.SaveStatistic(model, CreateLogger()).Wait();
                context.SaveChanges();
            }
            using (var context = new AppDbContext(DbContextOptions!))
            {
                var report = context.Reports.FirstOrDefault();
                Assert.AreEqual(1, context.Reports.Count());
                Assert.IsNotNull(report);
                Assert.AreEqual(model.UserID, report.UserID);
                Assert.AreEqual(model.StartDate, report.StartDate);
                Assert.AreEqual(model.EndDate, report.EndDate);
            }

        }

        [TestMethod()]
        public void GetStatisticTest()
        {
            using (var context = new AppDbContext(DbContextOptions!))
            {
                foreach (var report in GetTestData())
                {
                    context.Reports.Add(report);
                }
                context.SaveChanges();
            }

            using (var context = new AppDbContext(DbContextOptions!))
            {
                using var dt_context = new DateTimeProviderContext(new DateTime(2024, 10, 15, 12, 10, 30));

                var cntr = new ReportController(context);
                var res = cntr.GetStatistic("123", CreateConfiguration(), CreateLogger()).Result;
                var response400 = res as Microsoft.AspNetCore.Http.HttpResults.BadRequest<string>;
                Assert.AreEqual(StatusCodes.Status400BadRequest, response400.StatusCode);

                res = cntr.GetStatistic("8cace8a4-7445-4d21-a5b5-fc330f9b3aaa", CreateConfiguration(), CreateLogger()).Result;
                var response_notfound = res as Microsoft.AspNetCore.Http.HttpResults.StatusCodeHttpResult;
                Assert.AreEqual(StatusCodes.Status204NoContent, response_notfound.StatusCode);

                res = cntr.GetStatistic("8cace8a4-7445-4d21-a5b5-fc330f9b3307", CreateConfiguration(), CreateLogger()).Result;
                var response_undone = res as Microsoft.AspNetCore.Http.HttpResults.Ok<ReportDto>;
                Assert.AreEqual(StatusCodes.Status200OK, response_undone.StatusCode);
                Assert.IsNull(response_undone.Value.Result);
                Assert.AreEqual(response_undone.Value.Query, "8cace8a4-7445-4d21-a5b5-fc330f9b3307");
                Assert.AreEqual(response_undone.Value.Percent, 2000.0 / 60);

                res = cntr.GetStatistic("9cace8a4-7445-4d21-a5b5-fc330f9b3307", CreateConfiguration(), CreateLogger()).Result;
                var response_done = res as Microsoft.AspNetCore.Http.HttpResults.Ok<ReportDto>;
                Assert.AreEqual(StatusCodes.Status200OK, response_done.StatusCode);
                Assert.IsNotNull(response_done.Value.Result);
                Assert.AreEqual(response_done.Value.Query, "9cace8a4-7445-4d21-a5b5-fc330f9b3307");
                Assert.AreEqual(response_done.Value.Percent, 100d);
                Assert.AreEqual(response_done.Value.Result.UserID, "9cbf468f-dfeb-4ddd-b04c-ccbf47d23de2");
                Assert.AreEqual(response_done.Value.Result.CountSignIn, 5);
            }
        }
    }
}