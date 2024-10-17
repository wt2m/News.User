using Microsoft.EntityFrameworkCore;
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
    internal abstract class AbstractRepository<TEntity> : IAbstractRepository<TEntity> where TEntity : PersistentData
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

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("Trying to retrieve an user from database with empty id.");


            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
                throw new Exception("Entity not found.");

            return entity!;
        }
    }
}
