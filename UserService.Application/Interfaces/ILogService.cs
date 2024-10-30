using UserService.Domain.Entities.Abstract;

namespace UserService.Application.Interfaces
{
    public interface ILogService
    {
        Task Log<TLog>(TLog log) where TLog : PersistentLog;
    }
}
