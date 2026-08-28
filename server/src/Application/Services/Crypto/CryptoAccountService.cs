using Audex.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audex.Application.DTO.Crypto;
using Audex.Application.Interfaces.Crypto;
using Audex.Application.Mappings;
using Audex.Infrastructure.Entities.Crypto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Audex.Application.Services.Crypto
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

        public async Task<CryptoAccountDto> GetByIdAsync(Guid id)
        {
            var cryptoAccount = await _cryptoAccountRepo.GetByIdAsync(id);
            return _mapper.Map(cryptoAccount);  
        }

        public async Task<IEnumerable<CryptoAccountDto>> GetAllAsync()
        {
            var cryptoAccounts = await _cryptoAccountRepo.GetAllAsync(include: GetFullHierarchyColumns);
            return _mapper.Map(cryptoAccounts);
        }

        public async Task UpdateAsync(CryptoAccountDto cryptoAccountDto)
        {
            var cryptoAccount = _mapper.Map(cryptoAccountDto);
            _cryptoAccountRepo.Update(cryptoAccount);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(CryptoAccountDto cryptoAccountDto)
        {
            var cryptoAccount = _mapper.Map(cryptoAccountDto);
            cryptoAccount.Id = Guid.NewGuid();
            await _cryptoAccountRepo.AddAsync(cryptoAccount);
            await _db.CommitAsync();
            return cryptoAccount.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _cryptoAccountRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        private IQueryable<CryptoAccount> GetFullHierarchyColumns(
            IQueryable<CryptoAccount> cryptoAccountQuery)
        {
            return cryptoAccountQuery
                .Include(cryptoAccount => cryptoAccount.CryptoProvider);
        }
    }
}
