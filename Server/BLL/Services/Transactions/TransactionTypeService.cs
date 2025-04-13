using AutoMapper;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Transactions
{
    public class TransactionTypeService: ITransactionTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly ITransactionRepository _transactionsRepo;
        //private readonly IRepository<TransactionType> _transactionTypeRepo;
        private readonly IMapper _mapper;
        public TransactionTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionsRepo = uow.CreateTransactionRepository();
            //_transactionTypeRepo = uow.CreateRepository<TransactionType>();
        }

        public async Task<IEnumerable<string>> GetAll()
        {
            var result = await _transactionsRepo.GetTypes();
            
            return _mapper.Map<IEnumerable<string>>(result);
        }

        //public async Task<IEnumerable<TransactionTypeDTO>> GetAll()
        //{
        //    var result = await _transactionTypeRepo.GetAll();

        //    return _mapper.Map<IEnumerable<TransactionTypeDTO>>(result);
        //}

        //public async Task<TransactionTypeDTO> Add(string name, string extension, IFormFile formFile)
        //{
        //    if (formFile.Length != 0)
        //    {
        //        var fileId = Guid.NewGuid();
        //        //var path = Path.Combine(Directory.GetCurrentDirectory(), $"{fileId}.{extension}");
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "images", $"{fileId}.{extension}");
        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            var copyTask = formFile.CopyToAsync(stream);
        //            var type = new TransactionTypeDTO()
        //            {
        //                Id = fileId,
        //                Name = name,
        //                Extension = extension
        //            };
        //            var addTransactionTask = _transactionTypeRepo.Add(_mapper.Map<TransactionType>(type));
        //            await Task.WhenAll(copyTask, addTransactionTask);
        //            _db.Commit();
        //            return type;
        //        }
        //    }
        //    return null;
        //}

        //public async Task Update(Guid id, string name, string extension, IFormFile formFile)
        //{
        //    var typeDTO = new TransactionTypeDTO()
        //    {
        //        Id = id,
        //        Extension = extension,
        //        Name = name
        //    };
        //    var type = _mapper.Map<TransactionType>(typeDTO);
        //    await _transactionTypeRepo.Update(type);
        //    _db.Commit();
        //}

        //public async Task Delete(Guid id) =>
        //    await _transactionTypeRepo.Delete(id);
    }
}
