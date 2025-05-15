using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.WebApi.Models.Transactions;

namespace MoneyManager.WebApi.Controllers.Transactions
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IMapper _mapper;
        public TransactionTypeController(ITransactionTypeService transactionTypeService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionTypeService = transactionTypeService;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionTypeModel>> GetAll()
        {
            var transactions = await _transactionTypeService.GetAll();
            return _mapper.Map<IEnumerable<TransactionTypeModel>>(transactions);
        }

        [HttpPut]
        public async Task<TransactionTypeModel> Add(TransactionTypeModel transactionType)
        {
            var transactionTypeDto = _mapper.Map<TransactionTypeDTO>(transactionType);
            var res = await _transactionTypeService.Add(transactionTypeDto);
            return _mapper.Map<TransactionTypeModel>(res);
        }

        [HttpPatch]
        public async Task Update(TransactionTypeModel transactionType)
        {
            var transactionTypeDto = _mapper.Map<TransactionTypeDTO>(transactionType);
            await _transactionTypeService.Update(transactionTypeDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _transactionTypeService.Delete(id);
    }
}