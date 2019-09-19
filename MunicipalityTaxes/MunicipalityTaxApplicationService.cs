namespace MunicipalityTaxes
{
    using System.IO;
    using System.Linq;
    using DatabaseAgent;
    using Exceptions;
    using Models;
    using Newtonsoft.Json;

    public class MunicipalityTaxApplicationService : IMunicipalityTaxApplicationService
    {
        private readonly IMunicipalityTaxDatabaseAgent _municipalityTaxDatabaseAgent;

        public MunicipalityTaxApplicationService(IMunicipalityTaxDatabaseAgent municipalityTaxDatabaseAgent)
        {
            _municipalityTaxDatabaseAgent = municipalityTaxDatabaseAgent;
        }

        public bool InsertNewMunicipalityTaxFromFile()
        {
            var input = ReadMunicipalityTaxFromFile();
            var insertedFlag = false;

            if (input?.municipalityTaxList != null && input.municipalityTaxList.Any())
            {
                foreach (var municipalityTax in input.municipalityTaxList)
                {
                    if (_municipalityTaxDatabaseAgent.InsertNewMunicipalityTax(municipalityTax)) insertedFlag = true;
                }
            }
            else
            {
                throw new FileEmptyException();
            }

            return insertedFlag;
        }

        private MunicipalityTaxList ReadMunicipalityTaxFromFile()
        {
            return JsonConvert.DeserializeObject<MunicipalityTaxList>(
                    File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Resources\\MunicipalityTaxInput.json")));
        }
    }
}
