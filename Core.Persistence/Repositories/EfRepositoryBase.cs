using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Repositories
{
    public class EfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId>,IRepository<TEntity,TEntityId> where TEntity : Entity<TEntityId> where TContext : DbContext
    {

        protected readonly TContext Context;

        public EfRepositoryBase(TContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        protected virtual void EditEntityPropertiesToAdd(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            EditEntityPropertiesToAdd(entity);
            await Context.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ICollection<TEntity>> AddRangeAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellationToken = default
        )
        {
            foreach (TEntity entity in entities)
                EditEntityPropertiesToAdd(entity);
            await Context.AddRangeAsync(entities, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entities;
        }

        protected virtual void EditEntityPropertiesToUpdate(TEntity entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            EditEntityPropertiesToUpdate(entity);
            Context.Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ICollection<TEntity>> UpdateRangeAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellationToken = default
        )
        {
            foreach (TEntity entity in entities)
                EditEntityPropertiesToUpdate(entity);
            Context.UpdateRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
            return entities;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default)
        {
            await SetEntityAsDeleted(entity, permanent, isAsync: true, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ICollection<TEntity>> DeleteRangeAsync(
            ICollection<TEntity> entities,
            bool permanent = false,
            CancellationToken cancellationToken = default
        )
        {
            await SetEntityAsDeleted(entities, permanent, isAsync: true, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entities;
        }

        public async Task<Paginate<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            if (orderBy != null)
                return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
            return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
        }

        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Paginate<TEntity>> GetListByDynamicAsync(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool withDeleted = false,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> queryable = Query();
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return await queryable.AnyAsync(cancellationToken);
        }

        public TEntity Add(TEntity entity)
        {
            EditEntityPropertiesToAdd(entity);
            Context.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                EditEntityPropertiesToAdd(entity);
            Context.AddRange(entities);
            Context.SaveChanges();
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            EditEntityPropertiesToAdd(entity);
            Context.Update(entity);
            Context.SaveChanges();
            return entity;
        }

        public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                EditEntityPropertiesToAdd(entity);
            Context.UpdateRange(entities);
            Context.SaveChanges();
            return entities;
        }

        public TEntity Delete(TEntity entity, bool permanent = false)
        {
            SetEntityAsDeleted(entity, permanent, isAsync: false).Wait();
            Context.SaveChanges();
            return entity;
        }

        public ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false)
        {
            SetEntityAsDeleted(entities, permanent, isAsync: false).Wait();
            Context.SaveChanges();
            return entities;
        }

        public TEntity? Get(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool withDeleted = false,
            bool enableTracking = true
        )
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            return queryable.FirstOrDefault(predicate);
        }

        public Paginate<TEntity> GetList(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true
        )
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            if (orderBy != null)
                return orderBy(queryable).ToPaginate(index, size);
            return queryable.ToPaginate(index, size);
        }

        public Paginate<TEntity> GetListByDynamic(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true
        )
        {
            IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return queryable.ToPaginate(index, size);
        }

        public bool Any(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool withDeleted = false
        )
        {
            IQueryable<TEntity> queryable = Query();
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return queryable.Any();
        }

        /*
         Bu yöntem, bir varlığı kalıcı veya yumuşak (soft delete) olarak silmeye yarar. Kalıcı silme, varlığı veritabanından tamamen kaldırırken; yumuşak silme, varlığı silinmiş olarak işaretler (veritabanında kalır). Yöntem, hem asenkron hem de senkron çalıştırılabilir.
        Kalıcı silme: permanent = true olduğunda varlık tamamen silinir.
        Yumuşak silme: permanent = false olduğunda silinmiş olarak işaretlenir.
        Asenkron ve senkron çalışma: isAsync parametresiyle asenkron veya senkron çalışmayı kontrol eder. 
         */

        protected async Task SetEntityAsDeleted(
            TEntity entity,
            bool permanent,
            bool isAsync = true,
            CancellationToken cancellationToken = default
        )
        {
            if (!permanent)
            {
                CheckHasEntityHaveOneToOneRelation(entity);
                if (isAsync)
                    await setEntityAsSoftDeleted(entity, isAsync, cancellationToken);
                else
                    setEntityAsSoftDeleted(entity, isAsync).Wait();
            }
            else
                Context.Remove(entity);
        }

        protected async Task SetEntityAsDeleted(
            IEnumerable<TEntity> entities,
            bool permanent,
            bool isAsync = true,
            CancellationToken cancellationToken = default
        )
        {
            foreach (TEntity entity in entities)
                await SetEntityAsDeleted(entity, permanent, isAsync, cancellationToken);
        }

        /*
        Bu metot, bir varlık üzerindeki ilişkili varlıklar için dinamik bir sorgu oluşturur ve yumuşak silinmiş (soft delete) olmayan ilişkili varlıkları döndürür.
        Sorgunun ilişkili varlık türüne göre dinamik olarak çalışması için CreateQuery<TElement> metodu kullanılır.
        */
        protected IQueryable<object>? GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
        {
            Type queryProviderType = query.Provider.GetType();
            MethodInfo createQueryMethod =
                queryProviderType
                    .GetMethods()
                    .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                    ?.MakeGenericMethod(navigationPropertyType)
                ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
            var queryProviderQuery = (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: [query.Expression])!;
            return queryProviderQuery.Where(x => !((EntityTimestamps)x).DeletedDate.HasValue);
        }
        /*
        Bu metot, bir varlığın bire bir ilişkisi olup olmadığını kontrol eder ve yumuşak silme sırasında bu ilişkilerin sorunlara yol açabileceğini kullanıcıya bildirir.
        Bire bir ilişkilerde yumuşak silme (soft delete) ile yeniden veri ekleme, aynı yabancı anahtar ile veri oluşturulmaya çalışıldığında çakışmalara neden olabilir.
        Bu nedenle, eğer böyle bir ilişki varsa bir hata fırlatılarak işlem durdurulur.
         */
        protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
        {
            IEnumerable<IForeignKey> foreignKeys = Context.Entry(entity).Metadata.GetForeignKeys();
            IForeignKey? oneToOneForeignKey = foreignKeys.FirstOrDefault(fk =>
                fk.IsUnique && fk.PrincipalKey.Properties.All(pk => Context.Entry(entity).Property(pk.Name).Metadata.IsPrimaryKey())
            );

            if (oneToOneForeignKey != null)
            {
                string relatedEntity = oneToOneForeignKey.PrincipalEntityType.ClrType.Name;
                IReadOnlyList<IProperty> primaryKeyProperties = Context.Entry(entity).Metadata.FindPrimaryKey()!.Properties;
                string primaryKeyNames = string.Join(", ", primaryKeyProperties.Select(prop => prop.Name));
                throw new InvalidOperationException(
                    $"Entity {entity.GetType().Name} has a one-to-one relationship with {relatedEntity} via the primary key ({primaryKeyNames}). Soft Delete causes problems if you try to create an entry again with the same foreign key."
                );
            }
        }

        protected virtual void EditEntityPropertiesToDelete(TEntity entity)
        {
            entity.DeletedDate = DateTime.UtcNow;
        }

        protected virtual void EditRelationEntityPropertiesToCascadeSoftDelete(EntityTimestamps entity)
        {
            entity.DeletedDate = DateTime.UtcNow;
        }

        protected virtual bool IsSoftDeleted(EntityTimestamps entity)
        {
            return entity.DeletedDate.HasValue;
        }


        /*
         Bu metot, bir varlığı ve onun ilişkili olduğu tüm varlıkları yumuşak silme işlemiyle işaretler.
         Yani veritabanından tamamen silinmezler, sadece "silinmiş" olarak kabul edilirler.
         Rekürsif bir şekilde çalışarak ilişkili tüm varlıklar için de aynı işlem yapılır.
         Asenkron veya senkron olarak çalışabilir.        
         */

        private async Task setEntityAsSoftDeleted(
            EntityTimestamps entity,
            bool isAsync = true,
            CancellationToken cancellationToken = default,
            bool isRoot = true
        )
        {
            if (IsSoftDeleted(entity))
                return;
            if (isRoot)
                EditEntityPropertiesToDelete((TEntity)entity);
            else
                EditRelationEntityPropertiesToCascadeSoftDelete(entity);

            var navigations = Context
                .Entry(entity)
                .Metadata.GetNavigations()
                .Where(x =>
                    x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade }
                )
                .ToList();
            foreach (INavigation? navigation in navigations)
            {
                if (navigation.TargetEntityType.IsOwned())
                    continue;
                if (navigation.PropertyInfo == null)
                    continue;

                object? navValue = navigation.PropertyInfo.GetValue(entity);
                if (navigation.IsCollection)
                {
                    if (navValue == null)
                    {
                        IQueryable query = Context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();

                        if (isAsync)
                        {
                            IQueryable<object>? relationLoaderQuery = GetRelationLoaderQuery(
                                query,
                                navigationPropertyType: navigation.PropertyInfo.GetType()
                            );
                            if (relationLoaderQuery is not null)
                                navValue = await relationLoaderQuery.ToListAsync(cancellationToken);
                        }
                        else
                            navValue = GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                                ?.ToList();

                        if (navValue == null)
                            continue;
                    }

                    foreach (object navValueItem in (IEnumerable)navValue)
                        await setEntityAsSoftDeleted((EntityTimestamps)navValueItem, isAsync, cancellationToken, isRoot: false);
                }
                else
                {
                    if (navValue == null)
                    {
                        IQueryable query = Context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();

                        if (isAsync)
                        {
                            IQueryable<object>? relationLoaderQuery = GetRelationLoaderQuery(
                                query,
                                navigationPropertyType: navigation.PropertyInfo.GetType()
                            );
                            if (relationLoaderQuery is not null)
                                navValue = await relationLoaderQuery.FirstOrDefaultAsync(cancellationToken);
                        }
                        else
                            navValue = GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                                ?.FirstOrDefault();

                        if (navValue == null)
                            continue;
                    }

                    await setEntityAsSoftDeleted((EntityTimestamps)navValue, isAsync, cancellationToken, isRoot: false);
                }
            }

            Context.Update(entity);
        }

    }
}
