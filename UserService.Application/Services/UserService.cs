using UserService.Application.Interfaces;
using UserService.Domain.Repositories;

namespace UserService.Application.Services
{
    public class UserService : AbstractUowService, IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public UserService(IUserRepository userRepository, IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            _userRepository = userRepository;
        }

        public async Task UpdateUserPreferencesAsync(Guid userId, string preference)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(preference))
                throw new ArgumentException("Invalid userId or preference");

            // Fetch the user entity from the repository
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            // Use a method on the entity to manipulate its state
            user.AddPreferences(preference);

            _userRepository.Update(user);

            await _unitOfWork.CompleteAsync();
        }
    }
}
