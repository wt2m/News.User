using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Common.Enums;
using UserService.Domain.Entities.Abstract;

namespace UserService.Domain.Entities
{
    public class UserActionsLogs : PersistentLog
    {
        public  string UserId { get; private set; }
        public string Action { get; private set; }
        public LogType LogType { get; private set; }

        public UserActionsLogs(string userId, string action, string details, LogType type) : base(details) { 
            UserId = userId;
            Action = action;
            LogType = type;
        }
    }
}
