using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Trainees.Models.Interfaces.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate); // ✅ ADD THIS LINE
        void AddOrUpdate(TEntity entity);
        void Add(TEntity entity);
        void Update(TEntity entityInDb, TEntity updatedEntity);
        void Delete(Expression<Func<TEntity, bool>> predicate);
    }

}
