namespace MunicipalityTaxes.UnitTest
{
    using System.ComponentModel.DataAnnotations;
    using Models.ValidationAttributes;
    using Xunit;

    public class PriorityAttributeTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void PriorityAttribute_IsValid_PriorityIsCorrect(int priority)
        {
            var priorityAttribute = new PriorityAttribute();
            var result = priorityAttribute.IsValid(priority);

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(7)]
        public void PriorityAttribute_IsValid_PriorityIsIncorrect(int priority)
        {
            var priorityAttribute = new PriorityAttribute();
            var result = priorityAttribute.GetValidationResult(priority, new ValidationContext(priority));

            Assert.True(result != ValidationResult.Success);
            Assert.Equal("Priority does not exist", result.ErrorMessage);
        }
    }
}