using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Debts
{
    public class DebtTagService : IDebtTagService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<DebtTag> _debtTagRepo;
        private readonly IRepository<DebtPayment> _debtPaymentRepo;
        private readonly IRepository<DebtToDebtTag> _debtToDebtTagRepo;
        private readonly ApplicationMapper _mapper;

        public DebtTagService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _debtTagRepo = uow.CreateRepository<DebtTag>();
            _debtPaymentRepo = uow.CreateRepository<DebtPayment>();
            _debtToDebtTagRepo = uow.CreateRepository<DebtToDebtTag>();
        }

        public async Task<IEnumerable<DebtTagDto>> GetAllAsync()
        {
            var debtTags = await _debtTagRepo.GetAllAsync(include: GetFullHierarchyColumns);

            return debtTags.Select(debtTag => new DebtTagDto
            {
                Id = debtTag.Id,
                Name = debtTag.Name,
                ColorHex = debtTag.ColorHex,
                UsageCount = debtTag.DebtAssociations?.Count ?? 0
            });
        }

        public async Task<DebtTagDto> GetByIdAsync(Guid id)
        {
            var debtTag = await _debtTagRepo.GetByIdAsync(id, include: GetFullHierarchyColumns);

            if (debtTag == null)
            {
                return null!;
            }

            return new DebtTagDto
            {
                Id = debtTag.Id,
                Name = debtTag.Name,
                ColorHex = debtTag.ColorHex,
                UsageCount = debtTag.DebtAssociations?.Count ?? 0
            };
        }

        public async Task<IEnumerable<DebtTagStatsDto>> GetStatsAsync()
        {
            var debtTags = await _debtTagRepo.GetAllAsync(include: GetFullHierarchyColumns);
            var debtPayments = await _debtPaymentRepo.GetAllAsync(filter: payment => !payment.IsPercentagePayment);

            var debtTagStats = new List<DebtTagStatsDto>();

            foreach (var debtTag in debtTags)
            {
                var associatedDebts = debtTag.DebtAssociations?.Select(association => association.Debt).Where(debt => debt != null).ToList() ?? new List<Debt>();
                var debtIds = associatedDebts.Select(debt => debt.Id).ToHashSet();

                var remainingAmount = associatedDebts.Sum(debt => debt.Amount);
                var paidAmount = debtPayments.Where(payment => debtIds.Contains(payment.DebtId)).Sum(payment => payment.Amount);
                var totalAmount = remainingAmount + paidAmount;
                // TODO: possible more than 1 currency
                var currencyName = associatedDebts.FirstOrDefault()?.Currency?.Name ?? string.Empty;

                debtTagStats.Add(new DebtTagStatsDto
                {
                    TagId = debtTag.Id,
                    TagName = debtTag.Name,
                    ColorHex = debtTag.ColorHex,
                    RemainingAmount = remainingAmount,
                    TotalPaid = paidAmount,
                    TotalAmount = totalAmount,
                    CurrencyName = currencyName
                });
            }

            return debtTagStats;
        }

        public async Task<Guid> AddAsync(DebtTagDto debtTagDto)
        {
            var debtTag = _mapper.Map(debtTagDto);

            if (debtTag.Id == Guid.Empty)
            {
                debtTag.Id = Guid.NewGuid();
            }

            await _debtTagRepo.AddAsync(debtTag);
            await _db.CommitAsync();

            return debtTag.Id;
        }

        public async Task UpdateAsync(DebtTagDto debtTagDto)
        {
            var debtTag = await _debtTagRepo.GetByIdAsync(debtTagDto.Id, disableTracking: false);
            if (debtTag != null)
            {
                debtTag.Name = debtTagDto.Name;
                debtTag.ColorHex = debtTagDto.ColorHex;
                _debtTagRepo.Update(debtTag);
                await _db.CommitAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _debtTagRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        public async Task AssignTagsToDebtAsync(Guid debtId, IEnumerable<Guid> tagIds)
        {
            var existingAssociations = await _debtToDebtTagRepo.GetAllAsync(dt => dt.DebtId == debtId, disableTracking: false);
            var desiredTagIds = tagIds?.ToHashSet() ?? new HashSet<Guid>();

            foreach (var assoc in existingAssociations)
            {
                if (!desiredTagIds.Contains(assoc.DebtTagId))
                {
                    await _debtToDebtTagRepo.DeleteAsync(assoc.Id);
                }
            }

            var remainingTagIds = existingAssociations
                .Where(assoc => desiredTagIds.Contains(assoc.DebtTagId))
                .Select(assoc => assoc.DebtTagId)
                .ToHashSet();

            foreach (var tagId in desiredTagIds)
            {
                if (!remainingTagIds.Contains(tagId))
                {
                    await _debtToDebtTagRepo.AddAsync(new DebtToDebtTag
                    {
                        Id = Guid.NewGuid(),
                        DebtId = debtId,
                        DebtTagId = tagId
                    });
                }
            }

            await _db.CommitAsync();
        }

        private IQueryable<DebtTag> GetFullHierarchyColumns(IQueryable<DebtTag> debtTagQuery)
        {
            return debtTagQuery
                .Include(debtTag => debtTag.DebtAssociations)
                    .ThenInclude(association => association.Debt)
                        .ThenInclude(debt => debt.Currency);
        }
    }
}
