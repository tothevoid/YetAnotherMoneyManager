using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using MoneyManager.WEB.Model;

namespace MoneyManager.BLL.Services.Entities
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
            await _accountTypeRepo.Update(accountType);
            _db.Commit();
        }

        public async Task<Guid> Add(AccountTypeDTO accountTypeDto)
        {
            var accountType = _mapper.Map<AccountType>(accountTypeDto);
            accountType.Id = Guid.NewGuid();
            await _accountTypeRepo.Add(accountType);
            _db.Commit();
            return accountType.Id;
        }

        public async Task Delete(Guid id)
        {
            await _accountTypeRepo.Delete(id);
            _db.Commit();
        }
    }
}