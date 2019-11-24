using AutoMapper;
using Microsoft.AspNetCore.Http;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public class TransactionTypeService: ITransactionTypeService
    {
        private readonly string _fileStoragePath = "./";
        private readonly IUnitOfWork _db;
        private readonly IRepository<TransactionType> _transactionTypeRepo;
        private readonly IMapper _mapper;
        public TransactionTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionTypeRepo = uow.CreateRepository<TransactionType>();
        }

        public async Task<IEnumerable<TransactionTypeDTO>> GetAll()
        {
            var result = await _transactionTypeRepo.GetAll();
            
            return _mapper.Map<IEnumerable<TransactionTypeDTO>>(result);
        }

        public async Task<TransactionTypeDTO> Add(string name, string extension, IFormFile formFile)
        {
            if (formFile.Length != 0)
            {
                var fileId = Guid.NewGuid();
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"{fileId}.{extension}");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    var copyTask = formFile.CopyToAsync(stream);
                    var type = new TransactionTypeDTO()
                    {
                        Id = fileId,
                        Name = name,
                        Extension = extension
                    };
                    var addTransactionTask = _transactionTypeRepo.Add(_mapper.Map<TransactionType>(type));
                    await copyTask;
                    await addTransactionTask;
                    return type;
                }
            }
            return null;
        }

        public async Task Update(Guid id, string name, string extension, IFormFile formFile)
        {
            var typeDTO = new TransactionTypeDTO()
            {
                Id = id,
                Extension = extension,
                Name = name
            };
            var type = _mapper.Map<TransactionType>(typeDTO);
            await _transactionTypeRepo.Update(type);
            _db.Commit();
        }

        public async Task Delete(Guid id) =>
            await _transactionTypeRepo.Delete(id);
    }
}
