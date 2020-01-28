using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Keeper
{
    public interface IStore<T>
    {
        bool Exists(int id);
        Task<bool> ExistsAsync(int id);

        T Fetch(int id);
        Task<T> FetchAsync(int id);

        ICollection<T> FetchAll();
        Task<ICollection<T>> FetchAllAsync();

        int Save(T entity);
        Task<int> SaveAsync(T entity);

        void Update(T entity);
        Task UpdateAsync(T entity);

        void Delete(int id);
        Task DeleteAsync(int id);
    }
}
