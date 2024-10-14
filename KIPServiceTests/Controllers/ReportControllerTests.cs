using Microsoft.VisualStudio.TestTools.UnitTesting;
using KIPService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using KIPService.DbContexts;
using KIPService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        private static DbContextOptions<AppDbContext>? DbContextOptions { get; set; }


        private Mock<AppDbContext> CreateDbContex()
        {
            var reports = GetTestData().AsQueryable<Report>();
            var dbset = new Mock<DbSet<Report>>();
            dbset.As<IQueryable<Report>>().Setup(s => s.Provider).Returns(reports.Provider);
            dbset.As<IQueryable<Report>>().Setup(s => s.ElementType).Returns(reports.ElementType);
            dbset.As<IQueryable<Report>>().Setup(s => s.Expression).Returns(reports.Expression);
            dbset.As<IQueryable<Report>>().Setup(s => s.GetEnumerator()).Returns(reports.GetEnumerator());
            var context = new Mock<AppDbContext>();
            context.Setup(s => s.Reports).Returns(dbset.Object);
            return context;
        }

        private Mock<ILogger<ReportController>> CreateLogger() => new Mock<ILogger<ReportController>>();

        private IEnumerable<Report> GetTestData()
        {
            yield return new Report
            {
                ReportID = Guid.Parse("8cace8a4-7445-4d21-a5b5-fc330f9b3307"),
                EndDate = DateTime.Now.AddDays(-5),
                StartDate = DateTime.Now.AddDays(-10),
                InitDate = DateTime.Now.AddSeconds(-50),
                UserID = "8cbf468f-dfeb-4ddd-b04c-ccbf47d23de2",
                SignInCount = 0,
            };
            yield return new Report
            {
                ReportID = Guid.Parse("9cace8a4-7445-4d21-a5b5-fc330f9b3307"),
                EndDate = DateTime.Now.AddDays(-2),
                StartDate = DateTime.Now.AddDays(-4),
                InitDate = DateTime.Now.AddSeconds(-80),
                UserID = "9cbf468f-dfeb-4ddd-b04c-ccbf47d23de2",
                SignInCount = 0,
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
                cntr.SaveStatistic(model, CreateLogger().Object).Wait();
                context.SaveChanges();
            }
            using (var context = new AppDbContext(DbContextOptions!))
            {
                var report = context.Reports.FirstOrDefault();
                Assert.IsNotNull(report);
                Assert.AreEqual(model.UserID, report.UserID);
                Assert.AreEqual(model.StartDate, report.StartDate);
                Assert.AreEqual(model.EndDate, report.EndDate);
                Assert.AreEqual(1, context.Reports.Count());

            }

        }
    }
}