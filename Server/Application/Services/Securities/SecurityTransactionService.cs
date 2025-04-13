using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Securities
{
    public class SecurityTransactionService : ISecurityTransactionService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<SecurityTransaction> _securityTransactionRepo;
        private readonly IMapper _mapper;
        public SecurityTransactionService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityTransactionRepo = uow.CreateRepository<SecurityTransaction>();
        }

        public async Task<IEnumerable<SecurityTransactionDTO>> GetAll()
        {
            var securityTransactions = await _securityTransactionRepo.GetAll();
            return _mapper.Map<IEnumerable<SecurityTransactionDTO>>(securityTransactions);
        }

        public async Task Update(SecurityTransactionDTO securityTypeDto)
        {
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityTypeDto);
            await _securityTransactionRepo.Update(securityTransaction);
            _db.Commit();
        }

        public async Task<Guid> Add(SecurityTransactionDTO securityDto)
        {
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityDto);
            securityTransaction.Id = Guid.NewGuid();
            await _securityTransactionRepo.Add(securityTransaction);
            _db.Commit();
            return securityTransaction.Id;
        }

        public async Task Delete(Guid id)
        {
            await _securityTransactionRepo.Delete(id);
            _db.Commit();
        }
    }
}
