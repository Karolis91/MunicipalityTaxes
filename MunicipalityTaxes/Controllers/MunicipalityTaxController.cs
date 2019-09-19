namespace MunicipalityTaxes.Controllers
{
    using System;
    using DatabaseAgent;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class MunicipalityTaxController : ControllerBase
    {
        private readonly IMunicipalityTaxDatabaseAgent _municipalityTaxDatabaseAgent;
        private readonly IMunicipalityTaxApplicationService _municipalityTaxApplicationService;

        public MunicipalityTaxController(IMunicipalityTaxDatabaseAgent municipalityTaxDatabaseAgent, IMunicipalityTaxApplicationService municipalityTaxApplicationService)
        {
            _municipalityTaxDatabaseAgent = municipalityTaxDatabaseAgent;
            _municipalityTaxApplicationService = municipalityTaxApplicationService;
        }

        /// <summary>
        /// Gets the tax for specific date and municipality.
        /// </summary>
        /// <response code="200">Successful call</response>
        /// <response code="204">The tax for requested date does not exist</response>
        // GET api/municipalitytax
        [HttpGet("{municipality}/{date}")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<decimal> GetMunicipalityTaxForDate(string municipality, DateTime date)
        {
            var result = _municipalityTaxDatabaseAgent.GetMunicipalityTaxForDate(municipality, date);

            if (result?.Tax != null)
            {
                return result.Tax;
            }

            return NoContent();
        }

        /// <summary>
        /// Inserts new tax.
        /// </summary>
        /// <response code="201">Data was inserted</response>
        /// <response code="400">If wrong input or data already exists</response> 
        // POST api/municipalitytax
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PostNewMunicipalityTax([FromBody] MunicipalityTax municipalityTax)
        {
            if (_municipalityTaxDatabaseAgent.InsertNewMunicipalityTax(municipalityTax))
            {
                return Created("MunicipalityTax", "Data was inserted");
            }

            return BadRequest("Data already exists");
        }

        /// <summary>
        /// Insert new taxes from file.
        /// </summary>
        /// <response code="201">Data from file was inserted</response>
        /// <response code="204">The entries already exists in database</response>
        /// <response code="500">The file does not exist or is empty</response>
        // POST api/municipalitytax/fromfile
        [HttpPost("fromfile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PostNewMunicipalityTaxFromFile()
        {
            if (_municipalityTaxApplicationService.InsertNewMunicipalityTaxFromFile())
            {
                return Created("MunicipalityTax", "Data was inserted");
            }

            return NoContent();
        }
    }
}
