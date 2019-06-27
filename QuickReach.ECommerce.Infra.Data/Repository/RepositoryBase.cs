using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Domain;
using Microsoft.EntityFrameworkCore;

namespace QuickReach.ECommerce.Infra.Data
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase //generic type constraint = must be reference (implement) to entity base class 
    {
        protected readonly ECommerceDbContext context;
        public RepositoryBase(ECommerceDbContext context)
        {
            this.context = context;
        }
        public virtual TEntity Create(TEntity newEntity)
        {
            this.context.Set<TEntity>() //create dbset for the generic entity
                        .Add(newEntity);
            this.context.SaveChanges(); //to reflect in database
            return newEntity;
        }

        public virtual void Delete(int entityId)
        {
            var entityToRemove = Retrieve(entityId);
            this.context.Remove<TEntity>(entityToRemove);
            this.context.SaveChanges();
        }

        public virtual TEntity Retrieve(int entityId)
        {
            var entity = this.context.Set<TEntity>()
                                     .AsNoTracking()
                                     .FirstOrDefault(c => c.ID == entityId);
            return entity;
        }

        public IEnumerable<TEntity> Retrieve(int skip = 0, int count = 10)
        {
            var result = this.context.Set<TEntity>()
                        .AsNoTracking()
                        .Skip(skip)
                        .Take(count)
                        .ToList();
            
            return result;
        }

        public TEntity Update(int entityId, TEntity entity)
        {
            this.context.Update<TEntity>(entity);
            this.context.SaveChanges();
            return entity;
        }
    }
}
