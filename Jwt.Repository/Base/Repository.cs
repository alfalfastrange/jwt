using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Jwt.Common.Base;
using Jwt.Repository.Contexts;

namespace Jwt.Repository.Repositories
{
    public abstract class Repository<T> where T : BaseEntity
    {
        public virtual T Insert(T entity)
        {
            using (var context = new ModelContext())
            {
                context.Entry(entity).State = EntityState.Added;
                context.SaveChanges();
                entity = context.Entry(entity).Entity;
            }
            return entity;
        }

        public virtual T GetById(long id)
        {
            T item;
            using (var context = new ModelContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();
                item = dbQuery.AsNoTracking().FirstOrDefault(x => x.Id == id);
            }
            return item;
        }

        public virtual IList<T> GetAll()
        {
            List<T> list;
            using (var context = new ModelContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();
                list = dbQuery.AsNoTracking().ToList();
            }
            return list;
        }

        public virtual T FindSingle(Func<T, bool> predicate, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item;
            using (var context = new ModelContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include(navigationProperty);
                }
                item = dbQuery.AsNoTracking().FirstOrDefault(predicate);
            }
            return item;
        }

        public virtual IList<T> FindList(Func<T, bool> predicate, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new ModelContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include(navigationProperty);
                }
                list = dbQuery.AsNoTracking().Where(predicate).ToList();
            }
            return list;
        }

        public virtual int Update(T entity)
        {
            int rowsAffected;
            using (var context = new ModelContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                rowsAffected = context.SaveChanges();
            }
            return rowsAffected;
        }

        public virtual int Delete(T entity)
        {
            int rowsDeleted;
            using (var context = new ModelContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                rowsDeleted = context.SaveChanges();
            }
            return rowsDeleted;
        }
    }
}