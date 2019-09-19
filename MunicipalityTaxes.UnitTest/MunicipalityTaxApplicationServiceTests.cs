namespace MunicipalityTaxes.UnitTest
{
    using DatabaseAgent;
    using Models;
    using Moq;
    using Xunit;

    public class MunicipalityTaxApplicationServiceTests
    {
        private readonly Mock<IMunicipalityTaxDatabaseAgent> _municipalityTaxDatabaseAgentMock;

        public MunicipalityTaxApplicationServiceTests()
        {
            _municipalityTaxDatabaseAgentMock = new Mock<IMunicipalityTaxDatabaseAgent>();
        }

        [Fact]
        public void MunicipalityTaxApplicationService_InsertNewMunicipalityTaxFromFile_InsertsData()
        {
            _municipalityTaxDatabaseAgentMock.Setup(x => x.InsertNewMunicipalityTax(It.IsAny<MunicipalityTax>())).Returns(true);

            var municipalityTaxApplicationService = new MunicipalityTaxApplicationService(_municipalityTaxDatabaseAgentMock.Object);
            var isMunicipalityFromFileInserted = municipalityTaxApplicationService.InsertNewMunicipalityTaxFromFile();

            Assert.True(isMunicipalityFromFileInserted);
        }

        [Fact]
        public void MunicipalityTaxApplicationService_InsertNewMunicipalityTaxFromFile_FalseDataAlreadyExists()
        {
            _municipalityTaxDatabaseAgentMock.Setup(x => x.InsertNewMunicipalityTax(It.IsAny<MunicipalityTax>())).Returns(false);

            var municipalityTaxApplicationService = new MunicipalityTaxApplicationService(_municipalityTaxDatabaseAgentMock.Object);
            var isMunicipalityFromFileInserted = municipalityTaxApplicationService.InsertNewMunicipalityTaxFromFile();

            Assert.False(isMunicipalityFromFileInserted);
        }
    }
}
