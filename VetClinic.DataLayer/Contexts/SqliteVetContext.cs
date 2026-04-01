using Microsoft.EntityFrameworkCore;

namespace VetClinic.DataLayer.Contexts
{
    public class SqliteVetContext : VetClinicContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string databasePath = Path.Combine(folder, "VetClinic.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }
}
