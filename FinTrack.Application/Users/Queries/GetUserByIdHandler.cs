using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;


namespace FinTrack.Application.Users.Queries;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetUserByIdHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = (await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)) ??
                throw new InvalidOperationException($"User with ID '{request.UserId}' was not found.");
            _logger.LogInformation("User with id {UserId} found successfully", request.UserId);
            return _mapper.Map<UserDto>(existingUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find user by ID {UserId}", request.UserId);
            throw;
        }
    }
}
