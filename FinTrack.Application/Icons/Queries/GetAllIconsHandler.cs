using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Queries;
public class GetAllIconsHandler : IRequestHandler<GetAllIconsQuery, List<IconDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllIconsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<IconDto>> Handle(GetAllIconsQuery request, CancellationToken cancellationToken)
    {
        var icons = await _unitOfWork.IconRepository.GetAll();
        return icons.Select(IconDto.FromIcon).ToList();
    }
}

