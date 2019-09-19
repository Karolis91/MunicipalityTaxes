namespace MunicipalityTaxes.DatabaseAgent
{
    using System;
    using Models;

    public interface IMunicipalityTaxDatabaseAgent
    {
        MunicipalityTax GetMunicipalityTaxForDate(string municipality, DateTime date);

        bool InsertNewMunicipalityTax(MunicipalityTax municipalityTax);
    }
}