using AutoMapper;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Interfaces.Integrations.Currency;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Services.Crypto
{
    public class CryptoAccountCryptocurrencyService: ICryptoAccountCryptocurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<CryptoAccountCryptocurrency> _cryptoAccountCryptocurrencyRepo;
        private readonly IMapper _mapper;

        public CryptoAccountCryptocurrencyService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _cryptoAccountCryptocurrencyRepo = uow.CreateRepository<CryptoAccountCryptocurrency>();
        }

        public async Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetAll()
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyRepo.GetAll();
            return _mapper.Map<IEnumerable<CryptoAccountCryptocurrencyDto>>(cryptoAccountCryptocurrencies);
        }

        public async Task Update(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrencyDto)
        {
            var cryptoAccountCryptocurrency = _mapper.Map<CryptoAccountCryptocurrency>(cryptoAccountCryptocurrencyDto);
            _cryptoAccountCryptocurrencyRepo.Update(cryptoAccountCryptocurrency);
            await _db.Commit();
        }

        public async Task<Guid> Add(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrencyDto)
        {
            var cryptoAccountCryptocurrency = _mapper.Map<CryptoAccountCryptocurrency>(cryptoAccountCryptocurrencyDto);
            cryptoAccountCryptocurrency.Id = Guid.NewGuid();
            await _cryptoAccountCryptocurrencyRepo.Add(cryptoAccountCryptocurrency);
            await _db.Commit();
            return cryptoAccountCryptocurrency.Id;
        }

        public async Task Delete(Guid id)
        {
            await _cryptoAccountCryptocurrencyRepo.Delete(id);
            await _db.Commit();
        }
    }
}
