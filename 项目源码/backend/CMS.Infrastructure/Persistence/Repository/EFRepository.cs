using CMS.App.Common.Interface;
using Microsoft.EntityFrameworkCore;
using CMS.Domain.Base;

namespace CMS.Infrastructure.Persistence.Repository;
public class EFRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly CMSDbContext _db;
    private readonly DbSet<T> _table;
    private readonly IUserSession _user;

    public EFRepository(CMSDbContext db, IUserSession user)
    {
        _db = db;
        _table = db.Set<T>();
        _user = user;
    }
    public IQueryable<T> Table => _table.AsNoTracking();


    public async Task<T> AddAsync(T entity)
    {
        entity.Id = new Guid();
        entity.CreatedBy = _user.Id;
        entity.UpdatedBy = _user.Id;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActived = true;
        entity.IsDeleted = false;

        await _table.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.Id = new Guid();
            entity.CreatedBy = _user.Id;
            entity.UpdatedBy = _user.Id;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActived = true;
            entity.IsDeleted = false;
        }

        await _table.AddRangeAsync(entities);
        await _db.SaveChangesAsync();
        return entities;
    }

    public async Task<T?> DeleteAsync(T entity, bool shoudHardDel = false)
    {
        if (shoudHardDel == true)
        {
            _table.Remove(entity);
            await _db.SaveChangesAsync();
        }
        else
        {
            entity.UpdatedBy = _user.Id;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = true;
            await UpdateAsync(entity);
            await _db.SaveChangesAsync();
        }

        return entity;
    }

    public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities, bool shoudHardDel = false)
    {
        if (shoudHardDel == true)
        {
            _table.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
        else
        {
            foreach (var entity in entities)
            {
                entity.UpdatedBy = _user.Id;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.IsDeleted = true;
            }

            await UpdateAsync(entities);

        }

        return entities;
    }

    public async Task<T?> GetById(Guid? id)
    {
        if (id is null)
        {
            return null;
        }
        return await _table.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IEnumerable<T> GetByPageIndex(int pageIndex = 1, int pageSize = 5)
    {
        return GetByPageIndex(_table,pageIndex,pageSize);
    }
    public IEnumerable<T> GetByPageIndex(IQueryable<T> dataSource,int pageIndex = 1, int pageSize = 5)
    {
        return dataSource
            .Skip((pageIndex - 1 ) * pageSize)
            .Take(pageSize)
            .ToList<T>();
    }
    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedBy = _user.Id;
        entity.UpdatedAt = DateTime.UtcNow;

        _table.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.UpdatedBy = _user.Id;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        _table.UpdateRange(entities);
        await _db.SaveChangesAsync();
        return entities;
    }
}
