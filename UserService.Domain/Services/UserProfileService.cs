using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using UserService.Domain.Entities;

namespace UserService.Domain.Services
{
    public class UserProfileService
    {
        public void UpdateUserProfile(ApplicationUser user, string newFullName)
        {
            user.UpdateProfile(newFullName);
        }
    }
}
