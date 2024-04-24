﻿using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using MediatR;

namespace FinTrack.Application.Categories.Commands;

public record CreateCategory(string Title, int IconId, TransactionType Type) : IRequest<CategoryDto>;
public class CreateCategoryHandler : IRequestHandler<CreateCategory, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingCategory = (await _unitOfWork.CategoryRepository.GetAll())
                .FirstOrDefault(c => c.Title.ToLowerInvariant() == request.Title.ToLowerInvariant());

            if (existingCategory is not null)
            {
                throw new InvalidOperationException($"Category '{request.Title}' already exists.");
            }

            Category category = new(request.Title, request.Type, request.IconId);

            var createdCategory = await _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return CategoryDto.FromCategory(createdCategory);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}