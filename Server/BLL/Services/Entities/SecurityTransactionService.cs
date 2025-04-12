using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;
using MoneyManager.DAL.Interfaces;

namespace MoneyManager.BLL.Services.Entities
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

        public IEnumerable<SecurityTransactionDto> GetAll()
        {
            var securityTransactions = _securityTransactionRepo.GetAll();
            return _mapper.Map<IEnumerable<SecurityTransactionDto>>(securityTransactions);
        }

        public async Task Update(SecurityTransactionDto securityTypeDto)
        {
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityTypeDto);
            await _securityTransactionRepo.Update(securityTransaction);
            _db.Commit();
        }

        public async Task<Guid> Add(SecurityTransactionDto securityDto)
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
