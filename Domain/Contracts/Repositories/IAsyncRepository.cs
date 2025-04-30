using Domain.Entities;
using Domain.Shared;
using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;

public interface IAsyncRepository<TEntity> where TEntity : BaseEntity
{

    Task<PagedList<TEntity>> GetAllAsync(RequestParameters? requestParameters, bool includeDeleted = false,
       params Expression<Func<TEntity, object>>[] includeExpressions);
    Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task SaveCnangesAsync();
}

