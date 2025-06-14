using AutoMapper;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Infrastructure.Entities.Debts;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Securities;
using Minio.DataModel.Notification;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Application.Services.Debts
{
    public class DebtService: IDebtService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Debt> _debtRepo;
        private readonly IMapper _mapper;

        public DebtService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _debtRepo = uow.CreateRepository<Debt>();
        }

        public async Task<IEnumerable<DebtDto>> GetAll(bool onlyActive)
        {
            var builder = new ComplexQueryBuilder<Debt>();

            if (onlyActive)
            {
                builder.AddFilter(debt => debt.Amount > 0);
            }

            builder.AddJoins(GetFullHierarchyColumns)
                .AddOrder((debt) => debt.Date, true);

            var debts = await _debtRepo.GetAll(builder.GetQuery());
            return _mapper.Map<IEnumerable<DebtDto>>(debts);
        }

        public async Task Update(DebtDto debtDto)
        {
            var debt = _mapper.Map<Debt>(debtDto);
            _debtRepo.Update(debt);
            await _db.Commit();
        }

        public async Task<Guid> Add(DebtDto debtDto)
        {
            var debt = _mapper.Map<Debt>(debtDto);
            debt.Id = Guid.NewGuid();
            
            await _debtRepo.Add(debt);
            await _db.Commit();
            
            return debt.Id;
        }

        public async Task Delete(Guid id)
        {
            await _debtRepo.Delete(id);
            await _db.Commit();
        }

        private IQueryable<Debt> GetFullHierarchyColumns(IQueryable<Debt> debtQuery)
        {
            return debtQuery
                .Include(debt => debt.Currency);
        }
    }
}
