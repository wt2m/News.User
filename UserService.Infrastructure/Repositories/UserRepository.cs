using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.Identity
{
    public class UserRepository : AbstractRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext _context) : base(_context)
        {

        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task UpdateUserPreferencesAsync(Guid userId, string preference)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if(user == null)
                return;
            
            user.AddPreferences(preference);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
