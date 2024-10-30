using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities.Abstract;

namespace UserService.Domain.Entities
{
    public class SystemErrorLog : PersistentLog
    {

        public string? EndPoint { get; private set; }
        public int StatusCode { get; private set; }

        public SystemErrorLog(string message, string endpoint, int statusCode) : base(message)
        {
            EndPoint = endpoint;
            StatusCode = statusCode;
        }

        
       
    }
}
