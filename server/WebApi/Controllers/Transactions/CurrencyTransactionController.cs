using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Models.Securities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.WebApi.Models.Transactions;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Transactions
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyTransactionController : ControllerBase
    {
        private readonly ICurrencyTransactionService _currencyTransactionService;
        private readonly IMapper _mapper;
        public CurrencyTransactionController(ICurrencyTransactionService currencyTransactionService, IMapper mapper)
        {
            _mapper = mapper;
            _currencyTransactionService = currencyTransactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<CurrencyTransactionModel>> GetAll()
        {
            var currencyTransactions = await _currencyTransactionService.GetAll();
            return _mapper.Map<IEnumerable<CurrencyTransactionModel>>(currencyTransactions);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<CurrencyTransactionModel>> GetById(Guid id)
        {
            var dto = await _currencyTransactionService.GetById(id);
            if (dto == null) return NotFound();
            return _mapper.Map<CurrencyTransactionModel>(dto);
        }

        [HttpGet(nameof(GetAllByAccountId))]
        public async Task<IEnumerable<CurrencyTransactionModel>> GetAllByAccountId([FromQuery] Guid accountId)
        {
            var dtos = await _currencyTransactionService.GetAllByAccountId(accountId);
            return _mapper.Map<IEnumerable<CurrencyTransactionModel>>(dtos);
        }

        [HttpPut]
        public async Task<Guid> Add(CurrencyTransactionModel currencyTransaction)
        {
            var currencyTransactionDto = _mapper.Map<CurrencyTransactionDto>(currencyTransaction);
            return await _currencyTransactionService.Add(currencyTransactionDto);
        }

        [HttpPatch]
        public async Task Update(CurrencyTransactionModel currencyTransaction)
        {
            var currencyTransactionDto = _mapper.Map<CurrencyTransactionDto>(currencyTransaction);
            await _currencyTransactionService.Update(currencyTransactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _currencyTransactionService.Delete(id);
    }
}
