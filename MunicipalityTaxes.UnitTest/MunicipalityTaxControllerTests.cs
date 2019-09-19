namespace MunicipalityTaxes.UnitTest
{
    using DatabaseAgent;
    using Models;
    using Moq;
    using System;
    using Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class MunicipalityTaxControllerTests
    {
        private readonly Mock<IMunicipalityTaxDatabaseAgent> _municipalityTaxDatabaseAgentMock;
        private readonly Mock<IMunicipalityTaxApplicationService> _municipalityTaxApplicationServiceMock;

        public MunicipalityTaxControllerTests()
        {
            _municipalityTaxDatabaseAgentMock = new Mock<IMunicipalityTaxDatabaseAgent>();
            _municipalityTaxApplicationServiceMock = new Mock<IMunicipalityTaxApplicationService>();
        }

        [Fact]
        public void MunicipalityTaxController_GetMunicipalityTaxForDate_ReturnsTax()
        {
            _municipalityTaxDatabaseAgentMock
                .Setup(x => x.GetMunicipalityTaxForDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new MunicipalityTax()
                {
                    Tax = 2.3m
                });

            var municipalityTaxController = new MunicipalityTaxController(_municipalityTaxDatabaseAgentMock.Object, _municipalityTaxApplicationServiceMock.Object);
            var response = municipalityTaxController.GetMunicipalityTaxForDate("Vilnius", DateTime.Now);

            Assert.IsType<decimal>(response.Value);
            Assert.Equal(2.3m, response.Value);
        }

        [Fact]
        public void MunicipalityTaxController_GetMunicipalityTaxForDate_TaxDoesNotExist()
        {
            _municipalityTaxDatabaseAgentMock
                .Setup(x => x.GetMunicipalityTaxForDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns((MunicipalityTax)null);

            var municipalityTaxController = new MunicipalityTaxController(_municipalityTaxDatabaseAgentMock.Object, _municipalityTaxApplicationServiceMock.Object);
            var response = municipalityTaxController.GetMunicipalityTaxForDate("Vilnius", DateTime.Now);

            Assert.IsType<NoContentResult>(response.Result);
        }

        [Fact]
        public void MunicipalityTaxController_PostNewMunicipalityTax_TaxWasInserted()
        {
            _municipalityTaxDatabaseAgentMock
                .Setup(x => x.InsertNewMunicipalityTax(It.IsAny<MunicipalityTax>()))
                .Returns(true);

            var municipalityTaxController = new MunicipalityTaxController(_municipalityTaxDatabaseAgentMock.Object, _municipalityTaxApplicationServiceMock.Object);
            var response = municipalityTaxController.PostNewMunicipalityTax(new MunicipalityTax());

            Assert.IsType<CreatedResult>(response);
        }

        [Fact]
        public void MunicipalityTaxController_PostNewMunicipalityTax_TaxAlreadyExists()
        {
            _municipalityTaxDatabaseAgentMock
                .Setup(x => x.InsertNewMunicipalityTax(It.IsAny<MunicipalityTax>()))
                .Returns(false);

            var municipalityTaxController = new MunicipalityTaxController(_municipalityTaxDatabaseAgentMock.Object, _municipalityTaxApplicationServiceMock.Object);
            var response = municipalityTaxController.PostNewMunicipalityTax(new MunicipalityTax());

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void MunicipalityTaxController_PostNewMunicipalityTaxFromFile_TaxWasInserted()
        {
            _municipalityTaxApplicationServiceMock
                .Setup(x => x.InsertNewMunicipalityTaxFromFile())
                .Returns(true);

            var municipalityTaxController = new MunicipalityTaxController(_municipalityTaxDatabaseAgentMock.Object, _municipalityTaxApplicationServiceMock.Object);
            var response = municipalityTaxController.PostNewMunicipalityTaxFromFile();

            Assert.IsType<CreatedResult>(response);
        }

        [Fact]
        public void MunicipalityTaxController_PostNewMunicipalityTaxFromFile_TaxAlreadyExists()
        {
            _municipalityTaxApplicationServiceMock
                .Setup(x => x.InsertNewMunicipalityTaxFromFile())
                .Returns(false);

            var municipalityTaxController = new MunicipalityTaxController(_municipalityTaxDatabaseAgentMock.Object, _municipalityTaxApplicationServiceMock.Object);
            var response = municipalityTaxController.PostNewMunicipalityTaxFromFile();

            Assert.IsType<NoContentResult>(response);
        }
    }
}
