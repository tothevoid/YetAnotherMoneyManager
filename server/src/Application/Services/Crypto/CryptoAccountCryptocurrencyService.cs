using Audex.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Audex.Application.DTO.Crypto;
using Audex.Application.Interfaces.Crypto;
using Audex.Application.Mappings;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Application.Services.Crypto
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
            var existing = await _cryptoAccountCryptocurrencyRepo.FindAsync(c =>
                c.Id != cryptoAccountCryptocurrencyDto.Id &&
                c.CryptoAccountId == cryptoAccountCryptocurrencyDto.CryptoAccountId &&
                c.CryptocurrencyId == cryptoAccountCryptocurrencyDto.CryptocurrencyId);

            if (existing != null)
            {
                throw new InvalidOperationException("This cryptocurrency is already added to the crypto account.");
            }

            var cryptoAccountCryptocurrency = _mapper.Map(cryptoAccountCryptocurrencyDto);
            _cryptoAccountCryptocurrencyRepo.Update(cryptoAccountCryptocurrency);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrencyDto)
        {
            var existing = await _cryptoAccountCryptocurrencyRepo.FindAsync(c =>
                c.CryptoAccountId == cryptoAccountCryptocurrencyDto.CryptoAccountId &&
                c.CryptocurrencyId == cryptoAccountCryptocurrencyDto.CryptocurrencyId);

            if (existing != null)
            {
                throw new InvalidOperationException("This cryptocurrency is already added to the crypto account.");
            }

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

        public async Task<decimal> GetTotalBalanceAsync()
        {
            var items = await _cryptoAccountCryptocurrencyRepo.GetAllAsync(
                include: GetFullHierarchyColumns);

            return items.Sum(c => c.Quantity * (c.Cryptocurrency != null ? c.Cryptocurrency.Price : 0));
        }

        public async Task<decimal> GetTotalBalanceByCryptoAccountAsync(Guid cryptoAccountId)
        {
            var items = await _cryptoAccountCryptocurrencyRepo.GetAllAsync(
                c => c.CryptoAccountId == cryptoAccountId,
                include: GetFullHierarchyColumns);

            return items.Sum(c => c.Quantity * (c.Cryptocurrency != null ? c.Cryptocurrency.Price : 0));
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
