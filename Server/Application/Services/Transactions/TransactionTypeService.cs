using System;
using AutoMapper;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Application.DTO.Transactions;

namespace MoneyManager.Application.Services.Transactions
{
    public class TransactionTypeService: ITransactionTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<TransactionType> _transactionTypeRepo;
        private readonly IMapper _mapper;
        public TransactionTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionTypeRepo = uow.CreateRepository<TransactionType>();
        }

        public async Task<IEnumerable<TransactionTypeDTO>> GetAll(bool onlyActive = false)
        {
            var result = onlyActive
                ? await _transactionTypeRepo.GetAll(transaction => transaction.Active)
                : await _transactionTypeRepo.GetAll();
            return _mapper.Map<IEnumerable<TransactionTypeDTO>>(result);
        }

        public async Task<TransactionTypeDTO> Add(TransactionTypeDTO transactionTypeDto)
        {
            var transactionType = _mapper.Map<TransactionType>(transactionTypeDto);
            transactionType.Id = Guid.NewGuid();
            await _transactionTypeRepo.Add(transactionType);
            await _db.Commit();
            return transactionTypeDto;
        }

        public async Task Update(TransactionTypeDTO transactionTypeDto)
        {
            var transactionType = _mapper.Map<TransactionType>(transactionTypeDto);
            _transactionTypeRepo.Update(transactionType);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _transactionTypeRepo.Delete(id);
            await _db.Commit();
        }

    }
}
