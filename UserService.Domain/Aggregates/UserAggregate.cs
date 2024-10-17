using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Domain.Aggregates
{
    public class UserAggregate
    {
        public User User { get; private set; }

        public UserAggregate(User user)
        {
            User = user;
        }

    }
}
