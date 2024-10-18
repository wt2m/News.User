
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserLogs
    {
        Task LogObservationAsync(UserActionsLogs observation);
    }
}
