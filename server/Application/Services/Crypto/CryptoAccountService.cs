using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Crypto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Application.Services.Crypto
{
    public class CryptoAccountService : ICryptoAccountService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<CryptoAccount> _cryptoAccountRepo;
        private readonly ApplicationMapper _mapper;

        public CryptoAccountService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _cryptoAccountRepo = uow.CreateRepository<CryptoAccount>();
        }

        public async Task<CryptoAccountDto> GetById(Guid id)
        {
            var cryptoAccount = await _cryptoAccountRepo.GetById(id);
            return _mapper.Map(cryptoAccount);  
        }

        public async Task<IEnumerable<CryptoAccountDto>> GetAll()
        {
            var cryptoAccounts = await _cryptoAccountRepo.GetAll(include: GetFullHierarchyColumns);
            return _mapper.Map(cryptoAccounts);
        }

        public async Task Update(CryptoAccountDto cryptoAccountDto)
        {
            var cryptoAccount = _mapper.Map(cryptoAccountDto);
            _cryptoAccountRepo.Update(cryptoAccount);
            await _db.Commit();
        }

        public async Task<Guid> Add(CryptoAccountDto currencyDto)
        {
            var cryptoAccount = _mapper.Map(currencyDto);
            cryptoAccount.Id = Guid.NewGuid();
            await _cryptoAccountRepo.Add(cryptoAccount);
            await _db.Commit();
            return cryptoAccount.Id;
        }

        public async Task Delete(Guid id)
        {
            await _cryptoAccountRepo.Delete(id);
            await _db.Commit();
        }

        private IQueryable<CryptoAccount> GetFullHierarchyColumns(
            IQueryable<CryptoAccount> cryptoAccountQuery)
        {
            return cryptoAccountQuery
                .Include(cryptoAccount => cryptoAccount.CryptoProvider);
        }
    }
}
