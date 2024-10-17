using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities.Abstract;

namespace UserService.Domain.Repositories
{
    public interface IAbstractRepository<TEntity> where TEntity : PersistentData
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(TEntity entity);
        void Update(TEntity entity);
        void Update(List<TEntity> entity);
        void Delete(TEntity entity);

        TEntity GetById(int id);
    }
}
