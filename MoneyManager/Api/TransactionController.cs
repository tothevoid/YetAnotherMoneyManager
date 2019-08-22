using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System;
using MoneyManager.DAL.Interfaces;
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
        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
            var transactions = await _transactionService.GetAll();
            return _mapper.Map<IEnumerable<TransactionModel>>(transactions);
        }

        [HttpPut]
        public async Task<Guid> Add(TransactionModel transaction)
        {
            var transactionDTO = _mapper.Map<TransactionDTO>(transaction);
            return await _transactionService.Add(transactionDTO);
        }

        [HttpDelete]
        public async Task Delete(TransactionModel transaction)
        {
            var transactionDTO = _mapper.Map<TransactionDTO>(transaction);
            await _transactionService.Delete(transactionDTO);
        }

        [HttpPatch]
        public async Task Update(TransactionModel transaction)
        {
            var transactionDTO = _mapper.Map<TransactionDTO>(transaction);
            await _transactionService.Update(transactionDTO);
        }
    }
}