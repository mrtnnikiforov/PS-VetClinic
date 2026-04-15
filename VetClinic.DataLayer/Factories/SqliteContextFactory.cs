using Microsoft.EntityFrameworkCore;
using VetClinic.DataLayer.Contexts;
using VetClinic.Model.Interfaces;

namespace VetClinic.DataLayer.Factories
{
    public class SqliteContextFactory : IDatabaseContextFactory
    {
        public DbContext CreateContext()
        {
            return new SqliteVetContext();
        }
    }
}
