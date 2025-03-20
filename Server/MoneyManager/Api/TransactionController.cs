using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.WEB.Model;
using AutoMapper;
using MoneyManager.BLL.DTO;

namespace MoneyManager.WEB.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionsService _transactionService;
        private readonly IMapper _mapper;
        public TransactionController(ITransactionsService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }
       
        [HttpGet]
        public IEnumerable<TransactionModel> GetAll(int month, int year)
        {
            var transactions = _transactionService.GetAll(month, year);
            return _mapper.Map<IEnumerable<TransactionModel>>(transactions);
        }

        [HttpPut]
        public async Task<Guid> Add(TransactionModel transaction)
        {
            var transactionDTO = _mapper.Map<TransactionDTO>(transaction);
            return await _transactionService.Add(transactionDTO);
        }

        [HttpPatch]
        public async Task<IEnumerable<UpdateAccountModel>> Update(TransactionModel updatedTransaction)
        {
            var updatedTransactionDTO = _mapper.Map<TransactionDTO>(updatedTransaction);
            var accountsToUpdate = await _transactionService.Update(updatedTransactionDTO);
            return _mapper.Map<IEnumerable<UpdateAccountModel>>(accountsToUpdate);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _transactionService.Delete(id);
        }
    }
}