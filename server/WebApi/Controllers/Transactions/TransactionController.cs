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
        public async Task<IEnumerable<TransactionModel>> GetAll(int month, int year, bool showSystem)
        {
            var transactions = await _transactionService.GetAll(month, year, showSystem);
            return _mapper.Map<IEnumerable<TransactionModel>>(transactions);
        }

        [HttpPut]
        public async Task<TransactionModel> Add(TransactionModel transaction)
        {
            var transactionDto = _mapper.Map<TransactionDTO>(transaction);
            var addedTransaction = await _transactionService.Add(transactionDto);
            return _mapper.Map<TransactionModel>(addedTransaction);
        }

        [HttpPatch]
        public async Task Update(TransactionModel updatedTransaction)
        {
            var transactionDto = _mapper.Map<TransactionDTO>(updatedTransaction);
            await _transactionService.Update(transactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _transactionService.Delete(id);
        }
    }
}