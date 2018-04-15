using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCoreBll
{
public 	interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task Insert(T obj);
        Task Update(T obj,Guid id);
        Task<int> Delete(Guid id);
        Task<T> GetByName(string name);
        Task<IEnumerable<T>> GetByUserId(Guid userId);
    }
}
