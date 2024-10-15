using Microsoft.VisualStudio.TestTools.UnitTesting;
using KIPService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace KIPService.Models.Tests
{
    [TestClass()]
    public class ReportTests
    {
        public static IEnumerable<object[]> IsProcessingSource()
        {
            yield return new object[] { new Report()
            {
                InitDate = new DateTime(2024,10,15,12,10,0),
                EndDate = DateTime.Now.AddDays(-1),
                StartDate = DateTime.Now.AddDays(-33),
                ReportID = Guid.NewGuid(),
                UserID = Guid.NewGuid().ToString(),
                SignInCount = 0,
            }, 60, 50.0, true };
            yield return new object[] { new Report()
            {
                InitDate = new DateTime(2024,10,15,12,10,10),
                EndDate = DateTime.Now.AddDays(-1),
                StartDate = DateTime.Now.AddDays(-33),
                ReportID = Guid.NewGuid(),
                UserID = Guid.NewGuid().ToString(),
                SignInCount = 0,
            }, 60, 2000.0 / 60 , true };
            yield return new object[] { new Report()
            {
                InitDate = new DateTime(2024,10,15,12,10,40),
                EndDate = DateTime.Now.AddDays(-1),
                StartDate = DateTime.Now.AddDays(-33),
                ReportID = Guid.NewGuid(),
                UserID = Guid.NewGuid().ToString(),
                SignInCount = 0,
            }, 60, 0, true };
            yield return new object[] { new Report()
            {
                InitDate = new DateTime(2024,10,15,12,8,40),
                EndDate = DateTime.Now.AddDays(-1),
                StartDate = DateTime.Now.AddDays(-33),
                ReportID = Guid.NewGuid(),
                UserID = Guid.NewGuid().ToString(),
                SignInCount = 0,
            }, 60, 100, false };
        }


        [TestMethod()]
        [DynamicData(nameof(IsProcessingSource), DynamicDataSourceType.Method)]
        public void IsProcessingTest(Report report, int process_time, double expected_prc, bool expected)
        {
            using var dt_context = new DateTimeProviderContext(new DateTime(2024,10,15,12,10,30));
            var res = report.IsProcessing(process_time, out var prc);
            Assert.AreEqual(expected, res);
            Assert.AreEqual(expected_prc, prc);

        }
    }
}