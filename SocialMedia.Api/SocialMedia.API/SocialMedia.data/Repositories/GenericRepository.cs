using Microsoft.EntityFrameworkCore;
using SocialMedia.data.Models;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SocialMediaDbContext _dbContext;

        public GenericRepository(SocialMediaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async virtual Task<TEntity> GetById(long id) => await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task Create(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);
        public async Task Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        public virtual IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression) => _dbContext.Set<TEntity>().Where(expression).AsQueryable();

        public virtual IQueryable<TEntity> GetAll() => _dbContext.Set<TEntity>().AsQueryable();

        public async Task<IQueryable<TEntity>> Select(Expression<Func<TEntity, TEntity>> selector) => _dbContext.Set<TEntity>().Select(selector).AsQueryable();
        public async Task Delete(long id)
        {
            var entity = await GetById(id);
            _dbContext.Set<TEntity>().Remove(entity);
        }
        public async Task CreateWithDBTransaction(TEntity entity)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            await _dbContext.Set<TEntity>().AddAsync(entity);

            _dbContext.SaveChanges();

            transaction.Commit();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (func != null)
            {
                query = func(query);
            }
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<TEntity>(items, totalCount, pageIndex, pageSize);
        }
    }
}
