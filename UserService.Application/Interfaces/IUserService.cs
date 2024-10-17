﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task UpdateUserPreferencesAsync(Guid userId, string preference);
    }
}
