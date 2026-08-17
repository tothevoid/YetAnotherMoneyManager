using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.Application.Interfaces.FileStorage;
using Microsoft.AspNetCore.Http;

namespace MoneyManager.Application.Services.Crypto
{
    public class CryptocurrencyService : ICryptocurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Cryptocurrency> _cryptocurrencyRepo;
        private readonly ApplicationMapper _mapper;

        private readonly IFileStorageService _fileStorageService;
        private const string _iconsBucket = "cryptocurrency";

        public CryptocurrencyService(IUnitOfWork uow, ApplicationMapper mapper, IFileStorageService fileStorageService)
        {
            _db = uow;
            _mapper = mapper;
            _cryptocurrencyRepo = uow.CreateRepository<Cryptocurrency>();
            _fileStorageService = fileStorageService;
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

        public async Task<string> GetIconUrlAsync(string iconKey)
        {
            return await _fileStorageService.GetFileUrlAsync(_iconsBucket, iconKey);
        }
    }
}
