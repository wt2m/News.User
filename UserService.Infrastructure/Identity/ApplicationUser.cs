using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Add any additional properties that your domain requires
        public string? FullName { get; private set; }
        


        // Parameterless constructor needed by EF Core
        public ApplicationUser()
        {
        }

        // Constructor for creating a new user
        public ApplicationUser(Guid id, string email, string username, string fullName)
        {
            Id = id; // New GUID for new users
            Email = email;
            UserName = username;
            FullName = fullName;
        }

        // Method to update user profile
        public void UpdateProfile(string fullName)
        {
            FullName = fullName;
        }

        

    }
}
