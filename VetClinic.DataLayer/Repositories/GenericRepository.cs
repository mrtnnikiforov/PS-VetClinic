using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VetClinic.Model.Interfaces;

namespace VetClinic.DataLayer.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly IDatabaseContextFactory _contextFactory;

        public GenericRepository(IDatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private DbContext GetContext()
        {
            var context = _contextFactory.CreateContext();
            context.Database.EnsureCreated();
            return context;
        }

        public List<T> GetAll()
        {
            using var context = GetContext();
            return context.Set<T>().ToList();
        }

        public List<T> GetAll(params string[] includes)
        {
            using var context = GetContext();
            return ApplyIncludes(context.Set<T>().AsQueryable(), includes).ToList();
        }

        public T? GetById(int id)
        {
            using var context = GetContext();
            return context.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            using var context = GetContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            using var context = GetContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = GetContext();
            var entity = context.Set<T>().Find(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        public List<T> Query(Expression<Func<T, bool>> predicate)
        {
            using var context = GetContext();
            return context.Set<T>().Where(predicate).ToList();
        }

        public List<T> Query(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            using var context = GetContext();
            return ApplyIncludes(context.Set<T>().Where(predicate), includes).ToList();
        }

        private IQueryable<T> ApplyIncludes(IQueryable<T> query, string[] includes)
        {
            foreach (var inc in includes)
                query = query.Include(inc);
            return query;
        }
    }
}
