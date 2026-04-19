using Microsoft.EntityFrameworkCore;
using VetClinic.DataLayer.Contexts;
using VetClinic.Model.Interfaces;

namespace VetClinic.DataLayer.Factories
{
    public class SqlServerContextFactory : IDatabaseContextFactory
    {
        public DbContext CreateContext()
        {
            return new SqlServerVetContext();
        }
    }
}
