using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Services
{
    public class MonthlySummaryService : IMonthlySummaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MonthlySummaryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task UpdateMonthlySummary(int accountId, int year, Month month, decimal transactionAmount)
        {
            var existingMonthlySummary = (await _unitOfWork.MonthlySummaryRepository
            .Filter(q => q.Where(ms => ms.AccountId == accountId && ms.Year == year && ms.Month == month)))
            .FirstOrDefault();

            if (existingMonthlySummary == null)
            {
                await CreateMonthlySummary(accountId, year, month, transactionAmount);
            }
            else
            {
                existingMonthlySummary.Income += transactionAmount > 0 ? transactionAmount : 0;
                existingMonthlySummary.Expenses += transactionAmount < 0 ? -transactionAmount : 0;
                existingMonthlySummary.Balance += transactionAmount;

                await _unitOfWork.MonthlySummaryRepository.Update(existingMonthlySummary.Id, existingMonthlySummary);
            }
        }

        public async Task CreateMonthlySummary(int accountId, int year, Month month, decimal transactionAmount)
        {
            var monthlySummary = new MonthlySummary(accountId)
            {
                Year = year,
                Month = month,
                Income = transactionAmount > 0 ? transactionAmount : 0,
                Expenses = transactionAmount < 0 ? -transactionAmount : 0,
                Balance = transactionAmount
            };

            await _unitOfWork.MonthlySummaryRepository.Add(monthlySummary);
        }

        public async Task<List<MonthlySummaryDto>> GetMonthlySummariesForYear(int accountId, int year)
        {
            var monthlySummaries = await _unitOfWork.MonthlySummaryRepository
                .Filter(query => query.Where(ms => ms.AccountId == accountId && ms.Year == year));

            return _mapper.Map<List<MonthlySummaryDto>>(monthlySummaries);
        }

        public async Task<MonthlySummaryDto?> GetMonthlySummary(int accountId, int year, Month month)
        {
            var monthlySummary = await _unitOfWork.MonthlySummaryRepository
                .GetSingle(query => query.Where(ms => ms.AccountId == accountId && ms.Year == year && ms.Month == month));

            return monthlySummary != null
                ? _mapper.Map<MonthlySummaryDto>(monthlySummary)
                : null;
        }

    }
}
