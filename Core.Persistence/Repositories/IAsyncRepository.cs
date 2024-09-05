﻿using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Repositories
{
    public interface IAsyncRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId> 
    {
        Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = true,
        bool withDeleted = false,
        CancellationToken cancellationToken = default
    );

        Task<Paginate<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool enableTracking = true,
            bool withDeleted = false,
            CancellationToken cancellationToken = default
        );

        Task<Paginate<TEntity>> GetListByDynamicAsync(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool enableTracking = true,
            bool withDeleted = false,
            CancellationToken cancellationToken = default
        );

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<TEntity> AddAsync(TEntity entity);

        Task<IList<TEntity>> AddRangeAsync(IList<TEntity> entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<IList<TEntity>> UpdateRangeAsync(IList<TEntity> entity);

        Task<TEntity> DeleteAsync(TEntity entity,bool permanent=false);

        Task<IList<TEntity>> DeleteRangeAsync(IList<TEntity> entity);
    }
}
