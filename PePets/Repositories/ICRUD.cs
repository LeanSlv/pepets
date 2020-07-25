using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public interface ICRUD<T> where T : class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByNameAsync(string name);
    }
}
