    using Microsoft.EntityFrameworkCore;

namespace VetClinic.Model.Interfaces
{
    public interface IDatabaseContextFactory
    {
        DbContext CreateContext();
    }
}
