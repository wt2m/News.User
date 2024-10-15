using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.DTOs
{
    public class AuthenticateUserRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
