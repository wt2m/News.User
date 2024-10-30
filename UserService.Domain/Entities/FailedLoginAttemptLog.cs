using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities.Abstract;

namespace UserService.Domain.Entities
{
    public class FailedLoginAttemptLog : PersistentLog
    {
        public Guid UserId { get; private set; }
        public string IpAddress { get; private set; }
        public DateTime AttemptedAt { get; private set; }

        public FailedLoginAttemptLog(Guid userId, string ipAddress, string detail) : base(detail)
        {
            UserId = userId;
            IpAddress = ipAddress;
            AttemptedAt = DateTime.Now;
        }
    }
}
