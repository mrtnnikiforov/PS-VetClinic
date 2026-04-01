using VetClinic.DataLayer.Contexts;
using VetClinic.Model.Interfaces;

namespace VetClinic.DataLayer.Factories
{
    public class SqlServerContextFactory : IDatabaseContextFactory
    {
        public object CreateContext()
        {
            return new SqlServerVetContext();
        }
    }
}
