using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities.Abstract
{
    public abstract class PersistentLog
    {
        public string Detail { get; protected set; }
        public DateTime RegisteredAt { get; private set; }

        public PersistentLog(string message) {
            Detail = message;
            RegisteredAt = DateTime.Now;
        }
    }
}
