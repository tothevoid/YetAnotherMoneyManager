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

        public async Task<IEnumerable<CryptocurrencyDto>> GetAll()
        {
            var cryptocurrencies = await _cryptocurrencyRepo.GetAll();
            return _mapper.Map(cryptocurrencies);
        }

        public async Task<CryptocurrencyDto> Add(CryptocurrencyDto cryptocurrencyDto, IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = _mapper.Map(cryptocurrencyDto);
            cryptocurrency.Id = Guid.NewGuid();

            if (cryptocurrencyIcon != null)
            {
                var key = cryptocurrency.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, cryptocurrencyIcon, key);
                cryptocurrency.IconKey = key;
            }

            await _cryptocurrencyRepo.Add(cryptocurrency);
            await _db.Commit();
            return _mapper.Map(cryptocurrency);
        }

        public async Task<CryptocurrencyDto> Update(CryptocurrencyDto cryptocurrencyDto, IFormFile cryptocurrencyIcon)
        {
            var existingCrypto = await _cryptocurrencyRepo.GetById(cryptocurrencyDto.Id);
            var cryptocurrency = _mapper.Map(cryptocurrencyDto);

            if (cryptocurrencyIcon != null)
            {
                var key = cryptocurrency.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, cryptocurrencyIcon, key);
                cryptocurrency.IconKey = key;
            }
            else if (string.IsNullOrEmpty(cryptocurrencyDto.IconKey) && existingCrypto != null && !string.IsNullOrEmpty(existingCrypto.IconKey))
            {
                await _fileStorageService.DeleteFile(_iconsBucket, existingCrypto.IconKey);
                cryptocurrency.IconKey = null;
            }

            _cryptocurrencyRepo.Update(cryptocurrency);
            await _db.Commit();
            return _mapper.Map(cryptocurrency);
        }

        public async Task Delete(Guid id)
        {
            var crypto = await _cryptocurrencyRepo.GetById(id);
            if (crypto != null && !string.IsNullOrEmpty(crypto.IconKey))
            {
                await _fileStorageService.DeleteFile(_iconsBucket, crypto.IconKey);
            }

            await _cryptocurrencyRepo.Delete(id);
            await _db.Commit();
        }

        public async Task<string> GetIconUrl(string iconKey)
        {
            return await _fileStorageService.GetFileUrl(_iconsBucket, iconKey);
        }
    }
}
