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
    public class CryptoProviderService : ICryptoProviderService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<CryptoProvider> _cryptoProviderRepo;
        private readonly IMapper _mapper;

        public CryptoProviderService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _cryptoProviderRepo = uow.CreateRepository<CryptoProvider>();
        }

        public async Task<IEnumerable<CryptoProviderDto>> GetAll()
        {
            var cryptoProviders = await _cryptoProviderRepo.GetAll();
            return _mapper.Map<IEnumerable<CryptoProviderDto>>(cryptoProviders);
        }

        public async Task Update(CryptoProviderDto cryptoProviderDto)
        {
            var cryptoProvider = _mapper.Map<CryptoProvider>(cryptoProviderDto);
            _cryptoProviderRepo.Update(cryptoProvider);
            await _db.Commit();
        }

        public async Task<Guid> Add(CryptoProviderDto cryptoProviderDto)
        {
            var cryptoProvider = _mapper.Map<CryptoProvider>(cryptoProviderDto);
            cryptoProvider.Id = Guid.NewGuid();
            await _cryptoProviderRepo.Add(cryptoProvider);
            await _db.Commit();
            return cryptoProvider.Id;
        }

        public async Task Delete(Guid id)
        {
            await _cryptoProviderRepo.Delete(id);
            await _db.Commit();
        }
    }
}
