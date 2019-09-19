namespace MunicipalityTaxes.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MunicipalityTaxContext : DbContext, IDbContext
    {
        public MunicipalityTaxContext(DbContextOptions<MunicipalityTaxContext> options) : base(options)
        {}

        public virtual DbSet<MunicipalityTax> MunicipalityTax { get; set; }
    }
}
