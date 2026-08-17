using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Transactions;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Transactions
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionsService _transactionService;
        private readonly WebApiMapper _mapper;
        public TransactionController(ITransactionsService transactionService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }
       
        [HttpGet]
        public async Task<IEnumerable<TransactionModel>> GetAll(int month, int year, bool showSystem)
        {
            var transactions = await _transactionService.GetAllAsync(month, year, showSystem);
            return _mapper.Map(transactions);
        }

        [HttpPut]
        public async Task<TransactionModel> Add(TransactionModel transaction)
        {
            var transactionDto = _mapper.Map(transaction);
            var addedTransaction = await _transactionService.AddAsync(transactionDto);
            return _mapper.Map(addedTransaction);
        }

        [HttpPatch]
        public async Task Update(TransactionModel updatedTransaction)
        {
            var transactionDto = _mapper.Map(updatedTransaction);
            await _transactionService.UpdateAsync(transactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _transactionService.DeleteAsync(id);
        }
    }
}