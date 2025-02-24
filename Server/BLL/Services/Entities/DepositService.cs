using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces.Entities;

namespace MoneyManager.BLL.Services.Entities
{
    public class DepositService : IDepositService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Deposit> _depositRepo;
        private readonly IMapper _mapper;
        public DepositService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _depositRepo = uow.CreateRepository<Deposit>();
        }

        public async Task<IEnumerable<DepositDTO>> GetAll()
        {
            var deposits = await _depositRepo.GetAll();
            return _mapper.Map<IEnumerable<DepositDTO>>(deposits.OrderByDescending(x => x.From));
        }

        public async Task<Guid> Add(DepositDTO deposit)
        {
            var mappedDeposit = _mapper.Map<Deposit>(deposit);
            mappedDeposit.Id = Guid.NewGuid();
            await _depositRepo.Add(mappedDeposit);
            _db.Commit();
            return mappedDeposit.Id;
        }

        public async Task Update(DepositDTO updateTransactionModel)
        {
            var deposit = _mapper.Map<Deposit>(updateTransactionModel);
            await _depositRepo.Update(deposit);
            _db.Commit();
        }

        public async Task Delete(DepositDTO deposit)
        {
            await _depositRepo.Delete(deposit.Id);
            _db.Commit();
        }
    }
}