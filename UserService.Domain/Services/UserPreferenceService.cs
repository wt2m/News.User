using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Domain.Services
{
    public class UserPreferenceService
    { 
        public void AddPreference(ApplicationUser user, string preference)
        {
            user.AddPreference(preference);
        }
    }
}
