using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Queries;
public class GetIconByIdHandler : IRequestHandler<GetIconByIdQuery, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIconByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IconDto> Handle(GetIconByIdQuery request, CancellationToken cancellationToken)
    {

        var existingIcon = (await _unitOfWork.IconRepository.Get(request.IconId)) ??
            throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");

        return IconDto.FromIcon(existingIcon);
    }
}

