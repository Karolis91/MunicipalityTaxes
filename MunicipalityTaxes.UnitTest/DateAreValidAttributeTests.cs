namespace MunicipalityTaxes.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Models;
    using Models.ValidationAttributes;
    using Xunit;

    public class DateAreValidAttributeTests
    {
        [Theory]
        [MemberData(nameof(ValidTestDates))]
        public void MunicipalityTaxDatabaseAgent_EnteredDatesAreValid_DatesAreValid(DateTime dateFrom, DateTime dateTo)
        {
            var municipalityTax = new MunicipalityTax()
            {
                DateFrom = dateFrom
            };
            var datesAreValidAttribute = new DatesAreValidAttribute();
            var result = datesAreValidAttribute.GetValidationResult(dateTo, new ValidationContext(municipalityTax));

            Assert.True(result == ValidationResult.Success);
        }

        [Theory]
        [MemberData(nameof(NotValidTestDates))]
        public void MunicipalityTaxDatabaseAgent_EnteredDatesAreValid_DatesAreNotValid(DateTime dateFrom, DateTime dateTo)
        {
            var municipalityTax = new MunicipalityTax()
            {
                DateFrom = dateFrom
            };
            var datesAreValidAttribute = new DatesAreValidAttribute();
            var result = datesAreValidAttribute.GetValidationResult(dateTo, new ValidationContext(municipalityTax));

            Assert.True(result != ValidationResult.Success);
            Assert.Equal("Only daily, weekly, monthly and yearly dates are allowed", result.ErrorMessage);

        }

        [Theory]
        [MemberData(nameof(EmptyTestDates))]
        public void MunicipalityTaxDatabaseAgent_EnteredDatesAreValid_DatesAreEmpty(DateTime dateFrom, DateTime dateTo)
        {
            var municipalityTax = new MunicipalityTax()
            {
                DateFrom = dateFrom
            };
            var datesAreValidAttribute = new DatesAreValidAttribute();
            var result = datesAreValidAttribute.GetValidationResult(dateTo, new ValidationContext(municipalityTax));

            Assert.True(result != ValidationResult.Success);
            Assert.Equal("DateTo and DateFrom must be filled in", result.ErrorMessage);

        }

        public static IEnumerable<object[]> ValidTestDates =>
            new List<object[]>
            {
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 5, 2) },
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 5, 8) },
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 6, 1) },
                new object[] { new DateTime(2019, 5, 1), new DateTime(2020, 5, 1) }
            };

        public static IEnumerable<object[]> NotValidTestDates =>
            new List<object[]>
            {
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 5, 1) },
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 5, 7) },
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 5, 31) },
                new object[] { new DateTime(2019, 5, 1), new DateTime(2019, 4, 30) }
            };

        public static IEnumerable<object[]> EmptyTestDates =>
            new List<object[]>
            {
                new object[] { null, new DateTime(2019, 5, 1) },
                new object[] { new DateTime(2019, 5, 1), null},
                new object[] { null, null }
            };
    }
}
