using System.Linq.Expressions;

namespace VetClinic.Model.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> GetAll(params string[] includes);
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        List<T> Query(Expression<Func<T, bool>> predicate);
        List<T> Query(Expression<Func<T, bool>> predicate, params string[] includes);
    }
}
