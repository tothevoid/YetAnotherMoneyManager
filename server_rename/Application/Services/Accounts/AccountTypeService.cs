using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Accounts
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<AccountType> _accountTypeRepo;
        private readonly IMapper _mapper;
        public AccountTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _accountTypeRepo = uow.CreateRepository<AccountType>();
        }

        public async Task<IEnumerable<AccountTypeDTO>> GetAll()
        {
            var transactions = await _accountTypeRepo.GetAll();
            return _mapper.Map<IEnumerable<AccountTypeDTO>>(transactions);
        }

        public async Task Update(AccountTypeDTO accountTypeDto)
        {
            var accountType = _mapper.Map<AccountType>(accountTypeDto);
            _accountTypeRepo.Update(accountType);
            await _db.Commit();
        }

        public async Task<Guid> Add(AccountTypeDTO accountTypeDto)
        {
            var accountType = _mapper.Map<AccountType>(accountTypeDto);
            accountType.Id = Guid.NewGuid();
            await _accountTypeRepo.Add(accountType);
            await _db.Commit();
            return accountType.Id;
        }

        public async Task Delete(Guid id)
        {
            await _accountTypeRepo.Delete(id);
            await _db.Commit();
        }
    }
}