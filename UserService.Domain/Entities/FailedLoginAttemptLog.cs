using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities
{
    public class FailedLoginAttemptLog
    {
        public Guid UserId { get; private set; }
        public string IpAddress { get; private set; }
        public DateTime AttemptedAt { get; private set; }

        public FailedLoginAttemptLog(Guid userId, string ipAddress)
        {
            UserId = userId;
            IpAddress = ipAddress;
            AttemptedAt = DateTime.Now;
        }
    }
}
