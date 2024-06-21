using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FinTrack.Application.Users.Commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IUnitOfWork unitOfWork, ILogger<CreateUserHandler> logger,
           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var user = await _unitOfWork.UserRepository.CreateUserAsync(request.Email, request.Password, request.FirstName, request.LastName);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, request.FirstName),
                    new Claim(ClaimTypes.Surname, request.LastName),
                };

                await _unitOfWork.UserRepository.AddClaimsAsync(user, claims);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("User with Email {UserEmail} created successfully.", user.Email);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create user with Email {UserEmail}.", request.Email);
                throw;
            }
        }
    }
}