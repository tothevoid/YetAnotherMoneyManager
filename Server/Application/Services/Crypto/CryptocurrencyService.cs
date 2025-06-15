using AutoMapper;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Interfaces.Integrations.Currency;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Services.Crypto
{
    public class CryptocurrencyService : ICryptocurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Cryptocurrency> _cryptocurrencyRepo;
        private readonly IMapper _mapper;

        public CryptocurrencyService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _cryptocurrencyRepo = uow.CreateRepository<Cryptocurrency>();
        }

        public async Task<IEnumerable<CryptocurrencyDto>> GetAll()
        {
            var cryptocurrencies = await _cryptocurrencyRepo.GetAll();
            return _mapper.Map<IEnumerable<CryptocurrencyDto>>(cryptocurrencies);
        }

        public async Task Update(CryptocurrencyDto cryptocurrencyDto)
        {
            var cryptocurrency = _mapper.Map<Cryptocurrency>(cryptocurrencyDto);
            _cryptocurrencyRepo.Update(cryptocurrency);
            await _db.Commit();
        }

        public async Task<Guid> Add(CryptocurrencyDto cryptocurrencyDto)
        {
            var cryptocurrency = _mapper.Map<Cryptocurrency>(cryptocurrencyDto);
            cryptocurrency.Id = Guid.NewGuid();
            await _cryptocurrencyRepo.Add(cryptocurrency);
            await _db.Commit();
            return cryptocurrency.Id;
        }

        public async Task Delete(Guid id)
        {
            await _cryptocurrencyRepo.Delete(id);
            await _db.Commit();
        }
    }
}
