using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Data.Domain.Entities;

namespace InventoryManagement.Data.Repositories
{
    public abstract class Repository<TEntity, TKey>
               : IRepository<TEntity, TKey> where TKey : IComparable
               where TEntity : class, IEntity<TKey>
    {
        private DbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task RemoveAsync(TKey id)
        {
            var entityToDelete = _dbSet.Find(id);
            await RemoveAsync(entityToDelete);
        }

        public virtual async Task RemoveAsync(TEntity entityToDelete)
        {
            await Task.Run(() =>
            {
                if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            });
        }

        public virtual async Task RemoveAsync(Expression<Func<TEntity, bool>> filter)
        {
            await Task.Run(() =>
            {
                _dbSet.RemoveRange(_dbSet.Where(filter));
            });
        }

        public virtual async Task EditAsync(TEntity entityToUpdate)
        {
            await Task.Run(() =>
            {
                _dbSet.Attach(entityToUpdate);
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            });
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

		public virtual async Task<(IList<TEntity> data, int total, int totalDisplay)> GetDynamicAsync(
			Expression<Func<TEntity, bool>> filter = null,
			string orderBy = null,
			Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
			int pageIndex = 1,
			int pageSize = 10,
			bool isTrackingOff = false)
		{
			IQueryable<TEntity> query = _dbSet;
			var total = query.Count();
			var totalDisplay = query.Count();

			if (filter != null)
			{
				query = query.Where(filter);
				totalDisplay = query.Count();
			}

			if (include != null)
				query = include(query);

			IList<TEntity> data;

			if (orderBy != null)
			{
				var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);

				if (isTrackingOff)
					data = await result.AsNoTracking().ToListAsync();
				else
					data = await result.ToListAsync();
			}
			else
			{
				var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

				if (isTrackingOff)
					data = await result.AsNoTracking().ToListAsync();
				else
					data = await result.ToListAsync();
			}

			return (data, total, totalDisplay);
		}

		public virtual async Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isTrackingOff = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
                query = include(query);

            if (orderBy != null)
            {
                var result = orderBy(query);

                if (isTrackingOff)
                    return await result.AsNoTracking().ToListAsync();
                else
                    return await result.ToListAsync();
            }
            else
            {
                if (isTrackingOff)
                    return await query.AsNoTracking().ToListAsync();
                else
                    return await query.ToListAsync();
            }
        }
    }
}
