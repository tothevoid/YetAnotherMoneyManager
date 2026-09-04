using Audex.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audex.Application.DTO.Crypto;
using Audex.Application.DTO.Currencies;
using Audex.Application.DTO.FileStorage;
using Audex.Application.Interfaces.Crypto;
using Audex.Application.Interfaces.Currencies;
using Audex.Application.Mappings;
using Audex.Infrastructure.Constants;
using Audex.Infrastructure.Entities.Crypto;
using Audex.Application.Interfaces.FileStorage;
using Microsoft.AspNetCore.Http;

namespace Audex.Application.Services.Crypto
{
    public class CryptocurrencyService : ICryptocurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Cryptocurrency> _cryptocurrencyRepo;
        private readonly ApplicationMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrencyService _currencyService;
        private const string _iconsBucket = "cryptocurrency";

        public CryptocurrencyService(
            IUnitOfWork uow,
            ApplicationMapper mapper,
            IFileStorageService fileStorageService,
            ICurrencyService currencyService)
        {
            _db = uow;
            _mapper = mapper;
            _cryptocurrencyRepo = uow.CreateRepository<Cryptocurrency>();
            _fileStorageService = fileStorageService;
            _currencyService = currencyService;
        }

        public async Task<CurrencyDto> GetBaseCurrencyAsync()
        {
            var currency = await _currencyService.GetByIdAsync(CurrencyConstants.USD);
            if (currency == null)
            {
                throw new InvalidOperationException("Base cryptocurrency currency (USD) was not found.");
            }

            if (currency.Rate <= 0)
            {
                currency.Rate = 1.0m;
            }

            return currency;
        }

        public async Task<IEnumerable<CryptocurrencyDto>> GetAllAsync()
        {
            var cryptocurrencies = await _cryptocurrencyRepo.GetAllAsync();
            return _mapper.Map(cryptocurrencies);
        }

        public async Task<CryptocurrencyDto> AddAsync(CryptocurrencyDto cryptocurrencyDto, IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = _mapper.Map(cryptocurrencyDto);
            cryptocurrency.Id = Guid.NewGuid();

            if (cryptocurrencyIcon != null)
            {
                var key = $"{cryptocurrency.Id}_{Guid.NewGuid():N}";
                await _fileStorageService.UploadFileAsync(_iconsBucket, cryptocurrencyIcon, key);
                cryptocurrency.IconKey = key;
            }

            await _cryptocurrencyRepo.AddAsync(cryptocurrency);
            await _db.CommitAsync();
            return _mapper.Map(cryptocurrency);
        }

        public async Task<CryptocurrencyDto> UpdateAsync(CryptocurrencyDto cryptocurrencyDto, IFormFile cryptocurrencyIcon)
        {
            var existingCrypto = await _cryptocurrencyRepo.GetByIdAsync(cryptocurrencyDto.Id);
            var cryptocurrency = _mapper.Map(cryptocurrencyDto);

            if (cryptocurrencyIcon != null)
            {
                cryptocurrency.IconKey = $"{cryptocurrency.Id}_{Guid.NewGuid():N}";
                await _fileStorageService.UploadFileAsync(_iconsBucket, cryptocurrencyIcon, cryptocurrency.IconKey);
            }
            else if (string.IsNullOrEmpty(cryptocurrencyDto.IconKey))
            {
                cryptocurrency.IconKey = null;
            }

            if (!string.IsNullOrEmpty(existingCrypto?.IconKey) && existingCrypto.IconKey != cryptocurrency.IconKey)
            {
                await _fileStorageService.DeleteFileAsync(_iconsBucket, existingCrypto.IconKey);
            }

            _cryptocurrencyRepo.Update(cryptocurrency);
            await _db.CommitAsync();
            return _mapper.Map(cryptocurrency);
        }

        public async Task DeleteAsync(Guid id)
        {
            var crypto = await _cryptocurrencyRepo.GetByIdAsync(id);
            if (crypto != null && !string.IsNullOrEmpty(crypto.IconKey))
            {
                await _fileStorageService.DeleteFileAsync(_iconsBucket, crypto.IconKey);
            }

            await _cryptocurrencyRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        public async Task<FileStreamDto> GetIconStreamAsync(string iconKey)
        {
            return await _fileStorageService.GetFileStreamAsync(_iconsBucket, iconKey);
        }

        public async Task<string> GetIconUrlAsync(string iconKey)
        {
            return await _fileStorageService.GetFileUrlAsync(_iconsBucket, iconKey);
        }
    }
}
