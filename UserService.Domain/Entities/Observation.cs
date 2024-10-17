using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Common.Enums;

namespace UserService.Domain.Entities
{
    public class Observation
    {
        public  string UserId { get; private set; }
        public string Action { get; private set; }
        public string Details { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public LogType LogType { get; private set; }

        public Observation(string userId, string action, string details, LogType type) { 
            UserId = userId;
            Action = action;
            Details = details;
            LogType = type;
            CreatedAt = DateTime.Now;
        }
    }
}
