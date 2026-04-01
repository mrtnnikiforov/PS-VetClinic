using VetClinic.DataLayer.Contexts;
using VetClinic.Model.Interfaces;

namespace VetClinic.DataLayer.Factories
{
    public class SqliteContextFactory : IDatabaseContextFactory
    {
        public object CreateContext()
        {
            return new SqliteVetContext();
        }
    }
}
