using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jwt.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Insert(T entity);

        T GetById(long id);

        IList<T> GetAll();

        T FindSingle(Func<T, bool> predicate, params Expression<Func<T, object>>[] navigationProperties);

        IList<T> FindList(Func<T, bool> predicate, params Expression<Func<T, object>>[] navigationProperties);

        int Update(T entity);

        int Delete(T entity);
    }
}