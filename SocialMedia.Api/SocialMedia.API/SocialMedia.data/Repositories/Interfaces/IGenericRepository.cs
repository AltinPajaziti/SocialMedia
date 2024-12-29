using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);
        Task<IQueryable<TEntity>> Select(Expression<Func<TEntity, TEntity>> selector); // Define the Select method.
        Task<TEntity> GetById(long id);
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(long id);
        Task CreateWithDBTransaction(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllPagedAsync(
           Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
           int pageIndex = 1, int pageSize = int.MaxValue);
    }
}
