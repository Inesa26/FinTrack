using FinTrack.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Users.Commands
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserPasswordHandler> _logger;

        public UpdateUserPasswordHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserPasswordHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    _logger.LogError("User with ID {UserId} not found.", request.UserId);
                    throw new Exception($"User with ID {request.UserId} not found.");
                }

                // Verify old password
                var passwordValid = await _unitOfWork.UserRepository.CheckPasswordAsync(user, request.OldPassword);

                if (!passwordValid)
                {
                    _logger.LogError("Old password does not match for user with ID {UserId}.", request.UserId);
                    throw new Exception("Old password is incorrect.");
                }

                // Update password
                await _unitOfWork.UserRepository.SetPasswordAsync(user, request.NewPassword);

                // Commit transaction
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Password updated successfully for user with ID {UserId}.", user.Id);

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to update user with ID {UserId}.", request.UserId);
                throw;
            }
        }
    }
}
