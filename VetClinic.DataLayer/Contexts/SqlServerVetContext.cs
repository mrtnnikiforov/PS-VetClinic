using Microsoft.EntityFrameworkCore;

namespace VetClinic.DataLayer.Contexts
{
    public class SqlServerVetContext : VetClinicContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=VetClinic;Trusted_Connection=True;");
        }
    }
}
