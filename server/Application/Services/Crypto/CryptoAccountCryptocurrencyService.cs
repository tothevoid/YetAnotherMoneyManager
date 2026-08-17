using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Services.Crypto
{
    public class CryptoAccountCryptocurrencyService: ICryptoAccountCryptocurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<CryptoAccountCryptocurrency> _cryptoAccountCryptocurrencyRepo;
        private readonly ApplicationMapper _mapper;

        public CryptoAccountCryptocurrencyService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _cryptoAccountCryptocurrencyRepo = uow.CreateRepository<CryptoAccountCryptocurrency>();
        }

        public async Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetByCryptoAccountAsync(Guid cryptoAccountId)
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyRepo.GetAllAsync(
                (cryptoAccountCryptocurrency) => cryptoAccountCryptocurrency.CryptoAccountId == cryptoAccountId,
                include: GetFullHierarchyColumns);
            
            return _mapper.Map(cryptoAccountCryptocurrencies);
        }

        public async Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetAllAsync()
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyRepo.GetAllAsync(include: GetFullHierarchyColumns);
            return _mapper.Map(cryptoAccountCryptocurrencies);
        }

        public async Task UpdateAsync(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrencyDto)
        {
            var cryptoAccountCryptocurrency = _mapper.Map(cryptoAccountCryptocurrencyDto);
            _cryptoAccountCryptocurrencyRepo.Update(cryptoAccountCryptocurrency);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrencyDto)
        {
            var cryptoAccountCryptocurrency = _mapper.Map(cryptoAccountCryptocurrencyDto);
            cryptoAccountCryptocurrency.Id = Guid.NewGuid();
            await _cryptoAccountCryptocurrencyRepo.AddAsync(cryptoAccountCryptocurrency);
            await _db.CommitAsync();
            return cryptoAccountCryptocurrency.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _cryptoAccountCryptocurrencyRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        private IQueryable<CryptoAccountCryptocurrency> GetFullHierarchyColumns(
            IQueryable<CryptoAccountCryptocurrency> cryptoAccountCryptocurrencyQuery)
        {
            return cryptoAccountCryptocurrencyQuery
                .Include(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CryptoAccount.CryptoProvider)
                .Include(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.Cryptocurrency);
        }
    }
}
