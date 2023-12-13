using DataAccess.Model;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessAccess.Repository
{
  public interface IRepository<T> where T : BaseEntity
  {
    IQueryable<T> GetAll();
    IQueryable<T> Filter(Expression<Func<T, bool>> filter);
    T Get(Guid id, bool isActive = true);
    Task<T> GetAsync(Guid id, bool isActive = true);
    void Insert(T entity, bool saveChange = true);
    Task InsertAsync(T entity, bool saveChange = true);
    void Update(T entity, bool saveChange = true);
    Task UpdateAsync(T entity, bool saveChange = true);
    void Delete(T entity, bool isHardDelete = true , bool saveChange = true);
    Task DeleteAsync(T entity, bool isHardDelete = true, bool saveChange = true);
  }

}
