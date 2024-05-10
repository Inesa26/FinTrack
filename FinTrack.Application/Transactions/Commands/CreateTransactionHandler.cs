﻿using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Commands;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTransactionHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTransactionHandler(IUnitOfWork unitOfWork, ILogger<CreateTransactionHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

            Transaction transaction = new(request.Amount, request.Date, request.Description, request.CategoryId);

            var createdTransaction = await _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Transaction with ID {TransactionId} created successfully.", createdTransaction.Id);
            return _mapper.Map<TransactionDto>(createdTransaction);
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to create transaction.");
            throw;
        }
    }
}