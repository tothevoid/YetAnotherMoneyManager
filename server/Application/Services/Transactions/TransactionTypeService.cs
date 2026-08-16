using System;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Interfaces.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.FileStorage;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Services.FileStorage;

namespace MoneyManager.Application.Services.Transactions
{
    public class TransactionTypeService: ITransactionTypeService
    {
        private const string _iconsBucket = "transaction-type";

        private readonly IUnitOfWork _db;
        private readonly IRepository<TransactionType> _transactionTypeRepo;
        private readonly ApplicationMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public TransactionTypeService(IUnitOfWork uow, ApplicationMapper mapper, IFileStorageService fileStorageService)
        {
            _db = uow;
            _mapper = mapper;
            _transactionTypeRepo = uow.CreateRepository<TransactionType>();
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<TransactionTypeDTO>> GetAll(bool onlyActive = false)
        {
            var result = onlyActive
                ? await _transactionTypeRepo.GetAll(transaction => transaction.Active)
                : await _transactionTypeRepo.GetAll();
            return _mapper.Map(result);
        }

        public async Task<string> GetIconUrl(string iconKey)
        {
            return await _fileStorageService.GetFileUrl(_iconsBucket, iconKey);
        }

        public async Task<TransactionTypeDTO> Add(TransactionTypeDTO transactionTypeDto, IFormFile transactionTypeIcon)
        {
            var transactionType = _mapper.Map(transactionTypeDto);
            transactionType.Id = Guid.NewGuid();

            if (transactionTypeIcon != null)
            {
                var key = transactionTypeDto.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, transactionTypeIcon, key);
                transactionType.IconKey = key;
            }

            await _transactionTypeRepo.Add(transactionType);
            await _db.Commit();
            
            return _mapper.Map(transactionType);
        }

        public async Task<TransactionTypeDTO> Update(TransactionTypeDTO transactionTypeDto, IFormFile transactionTypeIcon)
        {
            var existingTransactionType = await _transactionTypeRepo.GetById(transactionTypeDto.Id);
            var transactionType = _mapper.Map(transactionTypeDto);

            if (transactionTypeIcon != null)
            {
                var key = transactionTypeDto.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, transactionTypeIcon, key);
                transactionType.IconKey = key;
            }
            else if (string.IsNullOrEmpty(transactionTypeDto.IconKey) && existingTransactionType != null && !string.IsNullOrEmpty(existingTransactionType.IconKey))
            {
                await _fileStorageService.DeleteFile(_iconsBucket, existingTransactionType.IconKey);
                transactionType.IconKey = null;
            }

            _transactionTypeRepo.Update(transactionType);
            await _db.Commit();

            return _mapper.Map(transactionType);
        }

        public async Task Delete(Guid id)
        {
            var transactionType = await _transactionTypeRepo.GetById(id);
            if (transactionType != null && !string.IsNullOrEmpty(transactionType.IconKey))
            {
                await _fileStorageService.DeleteFile(_iconsBucket, transactionType.IconKey);
            }

            await _transactionTypeRepo.Delete(id);
            await _db.Commit();
        }
    }
}
