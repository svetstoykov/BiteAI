using System.Linq.Expressions;

namespace BiteAI.Services.Interfaces.Base;

// Generic repository interface
public interface IRepository<T> where T : IEntity
{
    Task<T> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}