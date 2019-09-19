namespace MunicipalityTaxes.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public interface IDbContext
    {
        DbSet<MunicipalityTax> MunicipalityTax { get; set; }

        int SaveChanges();
    }
}
