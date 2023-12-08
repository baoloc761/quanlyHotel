using DataAccess.DBContext;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessAccess.Repository
{
  public class Repository<T> : IRepository<T> where T : BaseEntity
  {
    private readonly SampleNetCoreAPIContext context;
    private DbSet<T> entities;
    string errorMessage = string.Empty;

    public Repository(SampleNetCoreAPIContext context)
    {
      this.context = context;
      entities = context.Set<T>();
    }
    public IQueryable<T> GetAll()
    {
      return entities.AsQueryable();
    }

    public T Get(Guid id, bool isActive = true)
    {
      return entities.FirstOrDefault(s => s.Id == id && (s.Active || !isActive));
    }

    public async Task<T> GetAsync(Guid id, bool isActive = true)
    {
      return await entities.FirstOrDefaultAsync(s => s.Id == id && (s.Active || !isActive));
    }

    public IQueryable<T> Filter(Expression<Func<T, bool>> filter)
    {
      return entities.Where(filter);
    }

    public void Insert(T entity, bool saveChange = true)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }

      entity.CreatedTime = DateTime.Now;
      entity.UpdatedTime = DateTime.Now;

      entities.Add(entity);

      if (saveChange)
        context.SaveChanges();

    }

    public async Task InsertAsync(T entity, bool saveChange = true)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }

      entity.CreatedTime = DateTime.Now;
      entity.UpdatedTime = DateTime.Now;

      await entities.AddAsync(entity);

      if (saveChange)
        await context.SaveChangesAsync();

    }

    public void Update(T entity, bool saveChange = true)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }
      entity.UpdatedTime = DateTime.Now;
      if (saveChange)
        context.SaveChanges();
    }

    public async Task UpdateAsync(T entity, bool saveChange = true)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }
      entity.UpdatedTime = DateTime.Now;
      if (saveChange)
        await context.SaveChangesAsync();
    }

    public void Delete(T entity, bool saveChange = true)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }
      entities.Remove(entity);
      if (saveChange)
        context.SaveChanges();
    }

    public async Task DeleteAsync(T entity, bool saveChange = true)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }
      entities.Remove(entity);
      if (saveChange)
        await context.SaveChangesAsync();
    }
  }
}
