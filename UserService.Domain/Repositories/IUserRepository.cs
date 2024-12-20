﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Domain.Repositories
{
    public interface IUserRepository : IAbstractRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
