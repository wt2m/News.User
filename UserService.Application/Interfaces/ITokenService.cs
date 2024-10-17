using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(Guid userId);
        ClaimsPrincipal? VerifyTokenAsync(string token);

    }
}
