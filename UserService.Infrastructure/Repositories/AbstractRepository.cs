using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities.Abstract;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public abstract class AbstractRepository<TEntity> : IAbstractRepository<TEntity> where TEntity : PersistentData
    {
        protected readonly ApplicationDbContext _context;
        public AbstractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task AddRangeAsync(TEntity entity)
        {
            await _context.AddRangeAsync(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
                throw new Exception("The entity you tried to update is not on the database.");

            _context.Update(entity);
        }

        public void Update(List<TEntity> entity)
        {
            _context.UpdateRange(entity);
        }

        public void Delete(TEntity entity)
        {
            entity.ChangeToDeleted();
            _context.Update(entity);
        }

        public TEntity GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
