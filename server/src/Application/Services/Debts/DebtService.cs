using Audex.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audex.Application.DTO.Debts;
using Audex.Application.Interfaces.Debts;
using Audex.Application.Mappings;
using Audex.Infrastructure.Entities.Debts;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Queries;

namespace Audex.Application.Services.Debts
{
    public class DebtService : IDebtService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Debt> _debtRepo;
        private readonly ApplicationMapper _mapper;

        public DebtService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _debtRepo = uow.CreateRepository<Debt>();
        }

        public async Task<DebtDto> GetByIdAsync(Guid id)
        {
            var debt = await _debtRepo.GetByIdAsync(id, include: GetFullHierarchyColumns);
            return _mapper.Map(debt);
        }

        public async Task<IEnumerable<DebtDto>> GetAllAsync(bool onlyActive)
        {
            var builder = new ComplexQueryBuilder<Debt>();

            if (onlyActive)
            {
                builder.AddFilter(debt => debt.Amount > 0);
            }

            builder.AddJoins(GetFullHierarchyColumns)
                .AddOrder((debt) => debt.Date, true);

            var debts = await _debtRepo.GetAllAsync(builder.GetQuery());
            return _mapper.Map(debts);
        }

        public async Task UpdateAsync(DebtDto debtDto)
        {
            var debt = await _debtRepo.GetByIdAsync(debtDto.Id, disableTracking: false);

            if (debt == null) return;

            debt.Name = debtDto.Name;
            debt.Amount = debtDto.Amount;
            debt.CurrencyId = debtDto.CurrencyId;
            debt.Date = debtDto.Date;

            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(DebtDto debtDto)
        {
            var debt = _mapper.Map(debtDto);
            debt.Id = Guid.NewGuid();

            await _debtRepo.AddAsync(debt);
            await _db.CommitAsync();
            return debt.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _debtRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        private IQueryable<Debt> GetFullHierarchyColumns(IQueryable<Debt> debtQuery)
        {
            return debtQuery
                .Include(debt => debt.Currency)
                .Include(debt => debt.DebtTags)
                    .ThenInclude(dt => dt.DebtTag);
        }
    }
}
