namespace MunicipalityTaxes.UnitTest
{
    using System;
    using System.Linq;
    using DatabaseAgent;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Persistence;
    using Xunit;

    public class MunicipalityTaxDatabaseAgentTests
    {
        [Fact]
        public void MunicipalityTaxDatabaseAgent_GetMunicipalityTaxForDate_TaxDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<MunicipalityTaxContext>()
                .UseInMemoryDatabase(databaseName: "Get_empty_tax")
                .Options;

            using (var context = new MunicipalityTaxContext(options))
            {
                context.MunicipalityTax.Add(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 2),
                    DateTo = new DateTime(2019, 5, 3),
                    Priority = PriorityEnum.VeryHigh,
                    Tax = 2.3m,
                    Municipality = "Vilnius",
                    Id = 1
                });
                context.SaveChanges();
            }

            using (var context = new MunicipalityTaxContext(options))
            {
                var municipalityTaxDatabaseAgent = new MunicipalityTaxDatabaseAgent(context);
                var result = municipalityTaxDatabaseAgent.GetMunicipalityTaxForDate("Vilnius", new DateTime(2019, 5, 1));
                Assert.Null(result);
            }
        }

        [Fact]
        public void MunicipalityTaxDatabaseAgent_GetMunicipalityTaxForDate_FindsTheTax()
        {
            var options = new DbContextOptionsBuilder<MunicipalityTaxContext>()
                .UseInMemoryDatabase(databaseName: "Gets_inserted_tax")
                .Options;

            using (var context = new MunicipalityTaxContext(options))
            {
                context.MunicipalityTax.Add(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 1),
                    DateTo = new DateTime(2019, 5, 2),
                    Priority = PriorityEnum.VeryHigh,
                    Tax = 2.3m,
                    Municipality = "Vilnius",
                    Id = 1
                });
                context.SaveChanges();
            }

            using (var context = new MunicipalityTaxContext(options))
            {
                var municipalityTaxDatabaseAgent = new MunicipalityTaxDatabaseAgent(context);
                var result = municipalityTaxDatabaseAgent.GetMunicipalityTaxForDate("Vilnius", new DateTime(2019, 5, 1));
                Assert.Equal(2.3m, result.Tax);
            }
        }

        [Fact]
        public void MunicipalityTaxDatabaseAgent_GetMunicipalityTaxForDate_FindTheHighestPriorityTax()
        {
            var options = new DbContextOptionsBuilder<MunicipalityTaxContext>()
                .UseInMemoryDatabase(databaseName: "Gets_highest_priority_tax")
                .Options;

            using (var context = new MunicipalityTaxContext(options))
            {
                context.MunicipalityTax.Add(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 1),
                    DateTo = new DateTime(2019, 5, 2),
                    Priority = PriorityEnum.Low,
                    Tax = 2.3m,
                    Municipality = "Vilnius",
                    Id = 1
                });
                context.MunicipalityTax.Add(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 1),
                    DateTo = new DateTime(2019, 5, 2),
                    Priority = PriorityEnum.VeryHigh,
                    Tax = 2.5m,
                    Municipality = "Vilnius",
                    Id = 2
                });
                context.SaveChanges();
            }

            using (var context = new MunicipalityTaxContext(options))
            {
                var municipalityTaxDatabaseAgent = new MunicipalityTaxDatabaseAgent(context);
                var result = municipalityTaxDatabaseAgent.GetMunicipalityTaxForDate("Vilnius", new DateTime(2019, 5, 1));
                Assert.Equal(2.5m, result.Tax);
            }
        }

        [Fact]
        public void MunicipalityTaxDatabaseAgent_GetMunicipalityTaxForDate_FindTheLowestDurationTax()
        {
            var options = new DbContextOptionsBuilder<MunicipalityTaxContext>()
                .UseInMemoryDatabase(databaseName: "Gets_lowest_duration_tax")
                .Options;

            using (var context = new MunicipalityTaxContext(options))
            {
                context.MunicipalityTax.Add(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 1),
                    DateTo = new DateTime(2019, 5, 8),
                    Priority = PriorityEnum.Low,
                    Tax = 2.3m,
                    Municipality = "Vilnius",
                    Id = 1
                });
                context.MunicipalityTax.Add(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 1),
                    DateTo = new DateTime(2019, 6, 1),
                    Priority = PriorityEnum.VeryHigh,
                    Tax = 2.5m,
                    Municipality = "Vilnius",
                    Id = 2
                });
                context.SaveChanges();
            }

            using (var context = new MunicipalityTaxContext(options))
            {
                var municipalityTaxDatabaseAgent = new MunicipalityTaxDatabaseAgent(context);
                var result = municipalityTaxDatabaseAgent.GetMunicipalityTaxForDate("Vilnius", new DateTime(2019, 5, 5));
                Assert.Equal(2.3m, result.Tax);
            }
        }

        [Fact]
        public void MunicipalityTaxDatabaseAgent_GetMunicipalityTaxForDate_InsertsTax()
        {
            var options = new DbContextOptionsBuilder<MunicipalityTaxContext>()
                .UseInMemoryDatabase(databaseName: "Insert_new_tax")
                .Options;

            using (var context = new MunicipalityTaxContext(options))
            {
                var municipalityTaxDatabaseAgent = new MunicipalityTaxDatabaseAgent(context);
                var result = municipalityTaxDatabaseAgent.InsertNewMunicipalityTax(new MunicipalityTax()
                {
                    DateFrom = new DateTime(2019, 5, 1),
                    DateTo = new DateTime(2019, 5, 2),
                    Priority = PriorityEnum.Low,
                    Tax = 2.3m,
                    Municipality = "Vilnius",
                    Id = 1
                });

                Assert.True(result);

            }

            using (var context = new MunicipalityTaxContext(options))
            {
                Assert.Equal(1, context.MunicipalityTax.Count());
                Assert.Equal(2.3m, context.MunicipalityTax.Single().Tax);
            }
        }
    }
}
