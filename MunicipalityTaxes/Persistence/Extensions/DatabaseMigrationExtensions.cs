namespace MunicipalityTaxes.Persistence.Extensions
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseMigrationExtensions
    {
        public static void MigrateDatabase<TDbContext>(this TDbContext dbContext) where TDbContext : DbContext
        {
            try
            {
                var db = dbContext.Database;
                if (db.IsInMemory())
                {
                    return;
                }

                var migrate = false;
                foreach (var pendingMigration in db.GetPendingMigrations())
                {
                    migrate = true;
                    Console.WriteLine($"These migrations are pending and will be applied now: {pendingMigration}.");
                }

                if (migrate)
                {
                    Console.WriteLine("Applying DB migrations ...");
                    db.SetCommandTimeout(3 * 60);
                    db.Migrate();
                }
                else
                {
                    Console.WriteLine("No pending DB migrations.");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
