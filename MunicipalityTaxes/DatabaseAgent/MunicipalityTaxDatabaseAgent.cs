namespace MunicipalityTaxes.DatabaseAgent
{
    using System;
    using System.Linq;
    using Models;
    using Persistence;

    public class MunicipalityTaxDatabaseAgent : IMunicipalityTaxDatabaseAgent
    {
        private readonly IDbContext _context;

        public MunicipalityTaxDatabaseAgent(IDbContext context)
        {
            _context = context;
        }

        public MunicipalityTax GetMunicipalityTaxForDate(string municipality, DateTime date)
        {
            var result = _context.MunicipalityTax
                .Where(mt =>
                    mt.DateFrom.Date <= date.Date &&
                    mt.DateTo.Date > date.Date &&
                    string.Equals(mt.Municipality, municipality, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.DateTo - x.DateFrom)
                .ThenByDescending(y => y.Priority)
                .ThenBy(z => z.DateFrom)
                .FirstOrDefault();

                return result;
        }

        public bool InsertNewMunicipalityTax(MunicipalityTax municipalityTax)
        {
            var result = _context.MunicipalityTax
                .FirstOrDefault(mt => mt.DateFrom.Date == municipalityTax.DateFrom.Date &&
                    mt.DateTo.Date == municipalityTax.DateTo.Date &&
                    string.Equals(mt.Municipality, municipalityTax.Municipality, StringComparison.InvariantCultureIgnoreCase) &&
                    mt.Priority == municipalityTax.Priority);

            if (result != null)
            {
                return false;
            }

            _context.MunicipalityTax.Add(municipalityTax);
            _context.SaveChanges();

            return true;
        }
    }
}
