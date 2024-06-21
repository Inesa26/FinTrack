using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace FinTrack.Application.Users.Commands
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdatedUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly ITokenService _tokenService;

        public UpdateUserHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserHandler> logger, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<UpdatedUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;

                await _unitOfWork.UserRepository.UpdateUserAsync(user);

               
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                  
                };

                await _unitOfWork.UserRepository.ReplaceUserClaimsAsync(user, claims);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("User with ID {UserId} updated successfully.", user.Id);

                // Generate new token with updated user claims
                var updatedUserDto = _mapper.Map<UpdatedUserDto>(user);
                var token = _tokenService.GenerateToken(user);
                updatedUserDto.Token = token;
                return updatedUserDto;
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
