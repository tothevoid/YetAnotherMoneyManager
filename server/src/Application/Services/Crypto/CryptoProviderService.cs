using Audex.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audex.Application.DTO.Crypto;
using Audex.Application.Interfaces.Crypto;
using Audex.Application.Mappings;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Application.Services.Crypto
{
    public class CryptoProviderService : ICryptoProviderService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<CryptoProvider> _cryptoProviderRepo;
        private readonly ApplicationMapper _mapper;

        public CryptoProviderService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _cryptoProviderRepo = uow.CreateRepository<CryptoProvider>();
        }

        public async Task<IEnumerable<CryptoProviderDto>> GetAllAsync()
        {
            var cryptoProviders = await _cryptoProviderRepo.GetAllAsync();
            return _mapper.Map(cryptoProviders);
        }

        public async Task UpdateAsync(CryptoProviderDto cryptoProviderDto)
        {
            var cryptoProvider = _mapper.Map(cryptoProviderDto);
            _cryptoProviderRepo.Update(cryptoProvider);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(CryptoProviderDto cryptoProviderDto)
        {
            var cryptoProvider = _mapper.Map(cryptoProviderDto);
            cryptoProvider.Id = Guid.NewGuid();
            await _cryptoProviderRepo.AddAsync(cryptoProvider);
            await _db.CommitAsync();
            return cryptoProvider.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _cryptoProviderRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }
    }
}
