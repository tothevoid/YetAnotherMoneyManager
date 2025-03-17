using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Account> _accountRepo;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _accountRepo = uow.CreateRepository<Account>();
        }

        public async Task<IEnumerable<AccountDTO>> GetAll()
        {
            var transactions = await _accountRepo.GetAll();
            return _mapper.Map<IEnumerable<AccountDTO>>(transactions);
        }

        public async Task Update(AccountDTO accountDTO)
        {
            var account = _mapper.Map<Account>(accountDTO);
            await _accountRepo.Update(account);
            _db.Commit();
        }

        public async Task<Guid> Add(AccountDTO transactionDTO)
        {
            var account = _mapper.Map<Account>(transactionDTO);
            account.Id = Guid.NewGuid();
            await _accountRepo.Add(account);
            _db.Commit();
            return account.Id;
        }

        public async Task Delete(Guid id)
        {
            await _accountRepo.Delete(id);
            _db.Commit();
        }
    }
}