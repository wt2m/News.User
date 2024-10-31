using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.DependencyInjection
{
    public class DatabaseOptions
    {
        public bool UseInMemoryCache { get; set; } = false;
        public bool UseSQLServer { get; set; } = false;
    }
}
