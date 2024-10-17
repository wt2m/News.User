using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public abstract class AbstractUowService
    {
        public IUnitOfWork _unitOfWork;
        public AbstractUowService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
    }
}
