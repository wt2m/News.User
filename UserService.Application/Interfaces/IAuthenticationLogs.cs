
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IAuthenticationLogs
    {
        Task LogFailedLoginAsync(FailedLoginAttemptLog failedLoginLog);
    }
}
