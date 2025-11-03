using AutoMapper;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Interfaces.FileStorage;
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
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public BankService(IUnitOfWork uow, IMapper mapper, IFileStorageService fileStorageService)
        {
            _db = uow;
            _mapper = mapper;
            _bankRepo = uow.CreateRepository<Bank>();
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<BankDto>> GetAll()
        {
            var banks = await _bankRepo.GetAll();
            return _mapper.Map<IEnumerable<BankDto>>(banks);
        }

        public async Task<BankDto> GetById(Guid id)
        {
            var bank = await _bankRepo.GetById(id);
            return _mapper.Map<BankDto>(bank);
        }

        public async Task<BankDto> Add(BankDto bankDto, IFormFile bankIcon)
        {
            var bank = _mapper.Map<Bank>(bankDto);

            if (bankIcon != null)
            {
                var key = bank.Id.ToString();
                await _fileStorageService.UploadFile(IconsBucket, bankIcon, key);
                bank.IconKey = key;
            }

            await _bankRepo.Add(bank);
            await _db.Commit();
            return _mapper.Map<BankDto>(bank);
        }

        public async Task<BankDto> Update(BankDto bankDto, IFormFile bankIcon)
        {
            var bank = _mapper.Map<Bank>(bankDto);

            if (bankIcon != null)
            {
                var key = bank.Id.ToString();
                await _fileStorageService.UploadFile(IconsBucket, bankIcon, key);
                bank.IconKey = key;
            }
            
            _bankRepo.Update(bank);
            await _db.Commit();
            return _mapper.Map<BankDto>(bank);
        }

        public async Task<bool> Delete(Guid id)
        {
            var bank = await _bankRepo.GetById(id);

            await _bankRepo.Delete(id);
            await _db.Commit();

            return true;
        }

        public async Task<string> GetIconUrl(string iconKey)
        {
            return await _fileStorageService.GetFileUrl(IconsBucket, iconKey);
        }
    }
}