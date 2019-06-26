using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        TEntity Create(TEntity newEntity);
        TEntity Retrieve(int entityId);
        IEnumerable<TEntity> Retrieve(int skip = 0, int count = 10);
        TEntity Update(int entityId, TEntity entity);
        void Delete(int entityId);
    }
}
