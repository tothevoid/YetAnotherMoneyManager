using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.DTO.FileStorage;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Mappings;
using MoneyManager.Application.Services.FileStorage;
using MoneyManager.Infrastructure.Entities.Banks;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Banks
{
    public class BankService : IBankService
    {
        private const string IconsBucket = "bank";

        private readonly IUnitOfWork _db;
        private readonly IRepository<Bank> _bankRepo;
        private readonly ApplicationMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public BankService(IUnitOfWork uow, ApplicationMapper mapper, IFileStorageService fileStorageService)
        {
            _db = uow;
            _mapper = mapper;
            _bankRepo = uow.CreateRepository<Bank>();
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<BankDto>> GetAllAsync()
        {
            var banks = await _bankRepo.GetAllAsync();
            return _mapper.Map(banks);
        }

        public async Task<BankDto> GetByIdAsync(Guid id)
        {
            var bank = await _bankRepo.GetByIdAsync(id);
            return _mapper.Map(bank);
        }

        public async Task<BankDto> AddAsync(BankDto bankDto, IFormFile bankIcon)
        {
            var bank = _mapper.Map(bankDto);

            if (bankIcon != null)
            {
                var key = $"{bank.Id}_{Guid.NewGuid():N}";
                await _fileStorageService.UploadFileAsync(IconsBucket, bankIcon, key);
                bank.IconKey = key;
            }

            await _bankRepo.AddAsync(bank);
            await _db.CommitAsync();
            return _mapper.Map(bank);
        }

        public async Task<BankDto> UpdateAsync(BankDto bankDto, IFormFile bankIcon)
        {
            var existingBank = await _bankRepo.GetByIdAsync(bankDto.Id);
            var bank = _mapper.Map(bankDto);

            if (bankIcon != null)
            {
                bank.IconKey = $"{bank.Id}_{Guid.NewGuid():N}";
                await _fileStorageService.UploadFileAsync(IconsBucket, bankIcon, bank.IconKey);
            }
            else if (string.IsNullOrEmpty(bankDto.IconKey))
            {
                bank.IconKey = null;
            }

            if (!string.IsNullOrEmpty(existingBank?.IconKey) && existingBank.IconKey != bank.IconKey)
            {
                await _fileStorageService.DeleteFileAsync(IconsBucket, existingBank.IconKey);
            }
            
            _bankRepo.Update(bank);
            await _db.CommitAsync();
            return _mapper.Map(bank);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var bank = await _bankRepo.GetByIdAsync(id);
            if (bank != null && !string.IsNullOrEmpty(bank.IconKey))
            {
                await _fileStorageService.DeleteFileAsync(IconsBucket, bank.IconKey);
            }

            await _bankRepo.DeleteAsync(id);
            await _db.CommitAsync();

            return true;
        }

        public async Task<FileStreamDto> GetIconStreamAsync(string iconKey)
        {
            return await _fileStorageService.GetFileStreamAsync(IconsBucket, iconKey);
        }

        public async Task<string> GetIconUrlAsync(string iconKey)
        {
            return await _fileStorageService.GetFileUrlAsync(IconsBucket, iconKey);
        }
    }
}