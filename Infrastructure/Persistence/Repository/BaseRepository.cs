using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq.Expressions;
using System.Security.Claims;


namespace Infrastructure.Persistence.Repository;

public class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BaseRepository(AppDbContext dbContext,IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
        _httpContextAccessor = httpContextAccessor;
    }
    private string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task AddAsync(TEntity entity)
    {
        var userId = GetUserId();
        if (userId != null)
        {
            entity.SetCreated(userId);
        }
        await _dbSet.AddAsync(entity);
    }

    public async Task<PagedList<TEntity>> GetAllAsync(RequestParameters? requestParameters, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions)
    {
        var query = _dbSet.AsQueryable().AsNoTracking();
        var count = query.Count();
        var parameter = requestParameters == null ? new RequestParameters() : requestParameters; 
        if (includeExpressions.Length > 0)
            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }
        var result = await query
            .Skip((parameter.PageNumber - 1) * parameter.PageSize)
            .Take(parameter.PageSize)
            .ToListAsync();

        return PagedList<TEntity>.ToPagedList(result,count,parameter.PageNumber,parameter.PageSize);
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeExpressions)
    {
        var query = _dbSet.AsQueryable().AsNoTracking();
        if (includeExpressions.Length > 0)
            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }
       
        var result = await query.Where(predicate).FirstOrDefaultAsync();
        return result;
    }

    public async Task SaveCnangesAsync()
    {
         await _dbContext.SaveChangesAsync();       
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var userId = GetUserId();
        if (userId != null)
        {
            entity.SetModified(userId);
        }
        _dbSet.Update(entity);
    }
}
