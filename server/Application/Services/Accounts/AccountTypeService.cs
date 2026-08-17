using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Accounts
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<AccountType> _accountTypeRepo;
        private readonly ApplicationMapper _mapper;

        public AccountTypeService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _accountTypeRepo = uow.CreateRepository<AccountType>();
        }

        public async Task<IEnumerable<AccountTypeDto>> GetAllAsync()
        {
            var transactions = await _accountTypeRepo.GetAllAsync();
            return _mapper.Map(transactions);
        }

        public async Task UpdateAsync(AccountTypeDto accountTypeDto)
        {
            var accountType = _mapper.Map(accountTypeDto);
            _accountTypeRepo.Update(accountType);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(AccountTypeDto accountTypeDto)
        {
            var accountType = _mapper.Map(accountTypeDto);

            if (accountType.Id == Guid.Empty)
            {
                accountType.Id = Guid.NewGuid();
            }

            await _accountTypeRepo.AddAsync(accountType);
            await _db.CommitAsync();
            return accountType.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _accountTypeRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }
    }
}