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
        private readonly ApplicationMapper _mapper;

        public DebtTagService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _debtTagRepo = uow.CreateRepository<DebtTag>();
            _debtPaymentRepo = uow.CreateRepository<DebtPayment>();
        }

        public async Task<IEnumerable<DebtTagDto>> GetAll()
        {
            var debtTags = await _debtTagRepo.GetAll(include: GetFullHierarchyColumns);

            return debtTags.Select(debtTag => new DebtTagDto
            {
                Id = debtTag.Id,
                Name = debtTag.Name,
                ColorHex = debtTag.ColorHex,
                UsageCount = debtTag.DebtAssociations?.Count ?? 0
            });
        }

        public async Task<DebtTagDto> GetById(Guid id)
        {
            var debtTag = await _debtTagRepo.GetById(id, include: GetFullHierarchyColumns);

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

        public async Task<IEnumerable<DebtTagStatsDto>> GetStats()
        {
            var debtTags = await _debtTagRepo.GetAll(include: GetFullHierarchyColumns);
            var debtPayments = await _debtPaymentRepo.GetAll(filter: payment => !payment.IsPercentagePayment);

            var debtTagStats = new List<DebtTagStatsDto>();

            foreach (var debtTag in debtTags)
            {
                var associatedDebts = debtTag.DebtAssociations?.Select(association => association.Debt).Where(debt => debt != null).ToList() ?? new List<Debt>();
                var debtIds = associatedDebts.Select(debt => debt.Id).ToHashSet();

                var remainingAmount = associatedDebts.Sum(debt => debt.Amount);
                var paidAmount = debtPayments.Where(payment => debtIds.Contains(payment.DebtId)).Sum(payment => payment.Amount);
                var totalAmount = remainingAmount + paidAmount;

                debtTagStats.Add(new DebtTagStatsDto
                {
                    TagId = debtTag.Id,
                    TagName = debtTag.Name,
                    ColorHex = debtTag.ColorHex,
                    RemainingAmount = remainingAmount,
                    TotalPaid = paidAmount,
                    TotalAmount = totalAmount
                });
            }

            return debtTagStats;
        }

        public async Task<Guid> Add(DebtTagDto debtTagDto)
        {
            var debtTag = _mapper.Map(debtTagDto);

            if (debtTag.Id == Guid.Empty)
            {
                debtTag.Id = Guid.NewGuid();
            }

            await _debtTagRepo.Add(debtTag);
            await _db.Commit();

            return debtTag.Id;
        }

        public async Task Update(DebtTagDto debtTagDto)
        {
            var debtTag = await _debtTagRepo.GetById(debtTagDto.Id, disableTracking: false);
            if (debtTag != null)
            {
                debtTag.Name = debtTagDto.Name;
                debtTag.ColorHex = debtTagDto.ColorHex;
                _debtTagRepo.Update(debtTag);
                await _db.Commit();
            }
        }

        public async Task Delete(Guid id)
        {
            await _debtTagRepo.Delete(id);
            await _db.Commit();
        }

        private IQueryable<DebtTag> GetFullHierarchyColumns(IQueryable<DebtTag> debtTagQuery)
        {
            return debtTagQuery
                .Include(debtTag => debtTag.DebtAssociations)
                    .ThenInclude(association => association.Debt);
        }
    }
}
