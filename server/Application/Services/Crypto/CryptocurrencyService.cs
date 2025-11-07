using AutoMapper;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.Application.Interfaces.FileStorage;
using Microsoft.AspNetCore.Http;

namespace MoneyManager.Application.Services.Crypto
{
    public class CryptocurrencyService : ICryptocurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Cryptocurrency> _cryptocurrencyRepo;
        private readonly IMapper _mapper;

        private readonly IFileStorageService _fileStorageService;
        private const string _iconsBucket = "cryptocurrency";

        public CryptocurrencyService(IUnitOfWork uow, IMapper mapper, IFileStorageService fileStorageService)
        {
            _db = uow;
            _mapper = mapper;
            _cryptocurrencyRepo = uow.CreateRepository<Cryptocurrency>();
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<CryptocurrencyDto>> GetAll()
        {
            var cryptocurrencies = await _cryptocurrencyRepo.GetAll();
            return _mapper.Map<IEnumerable<CryptocurrencyDto>>(cryptocurrencies);
        }

        public async Task<CryptocurrencyDto> Add(CryptocurrencyDto cryptocurrencyDto, IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = _mapper.Map<Cryptocurrency>(cryptocurrencyDto);
            cryptocurrency.Id = Guid.NewGuid();

            if (cryptocurrencyIcon != null)
            {
                var key = cryptocurrency.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, cryptocurrencyIcon, key);
                cryptocurrency.IconKey = key;
            }

            await _cryptocurrencyRepo.Add(cryptocurrency);
            await _db.Commit();
            return _mapper.Map<CryptocurrencyDto>(cryptocurrency);
        }

        public async Task<CryptocurrencyDto> Update(CryptocurrencyDto cryptocurrencyDto, IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = _mapper.Map<Cryptocurrency>(cryptocurrencyDto);

            if (cryptocurrencyIcon != null)
            {
                var key = cryptocurrency.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, cryptocurrencyIcon, key);
                cryptocurrency.IconKey = key;
            }

            _cryptocurrencyRepo.Update(cryptocurrency);
            await _db.Commit();
            return _mapper.Map<CryptocurrencyDto>(cryptocurrency);
        }

        public async Task Delete(Guid id)
        {
            await _cryptocurrencyRepo.Delete(id);
            await _db.Commit();
        }

        public async Task<string> GetIconUrl(string iconKey)
        {
            return await _fileStorageService.GetFileUrl(_iconsBucket, iconKey);
        }
    }
}
