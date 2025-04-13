using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.WebApi.Models.Accounts;
using MoneyManager.WebApi.Models.Transactions;

namespace MoneyManager.WebApi.Controllers.Transactions
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