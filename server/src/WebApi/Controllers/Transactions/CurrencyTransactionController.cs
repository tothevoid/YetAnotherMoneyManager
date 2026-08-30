using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Audex.Application.DTO.Transactions;
using Audex.Application.Interfaces.Transactions;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Transactions;
using Microsoft.AspNetCore.Authorization;

using Audex.WebApi.Models.Common;

namespace Audex.WebApi.Controllers.Transactions
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyTransactionController : ControllerBase
    {
        private readonly ICurrencyTransactionService _currencyTransactionService;
        private readonly WebApiMapper _mapper;
        public CurrencyTransactionController(ICurrencyTransactionService currencyTransactionService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _currencyTransactionService = currencyTransactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<CurrencyTransactionModel>> GetAll()
        {
            var currencyTransactions = await _currencyTransactionService.GetAllAsync();
            return _mapper.Map(currencyTransactions);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<CurrencyTransactionModel>> GetById(Guid id)
        {
            var dto = await _currencyTransactionService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return _mapper.Map(dto);
        }

        [HttpGet(nameof(GetAllByAccountId))]
        public async Task<IEnumerable<CurrencyTransactionModel>> GetAllByAccountId(
            [FromQuery] Guid accountId,
            [FromQuery] int? pageIndex = null,
            [FromQuery] int? recordsQuantity = null)
        {
            var dtos = await _currencyTransactionService.GetAllByAccountIdAsync(accountId, pageIndex, recordsQuantity);
            return _mapper.Map(dtos);
        }

        [HttpGet(nameof(GetPaginationByAccountId))]
        public async Task<PaginationConfigModel> GetPaginationByAccountId([FromQuery] Guid accountId)
        {
            var pagination = await _currencyTransactionService.GetPaginationAsync(accountId);
            return _mapper.Map(pagination);
        }

        [HttpGet(nameof(GetSummaryByAccountId))]
        public async Task<CurrencyAccountSummaryModel> GetSummaryByAccountId([FromQuery] Guid accountId)
        {
            var dto = await _currencyTransactionService.GetSummaryByAccountIdAsync(accountId);
            return _mapper.Map(dto);
        }

        [HttpPut]
        public async Task<Guid> Add(CurrencyTransactionModel currencyTransaction)
        {
            var currencyTransactionDto = _mapper.Map(currencyTransaction);
            return await _currencyTransactionService.AddAsync(currencyTransactionDto);
        }

        [HttpPatch]
        public async Task Update(CurrencyTransactionModel currencyTransaction)
        {
            var currencyTransactionDto = _mapper.Map(currencyTransaction);
            await _currencyTransactionService.UpdateAsync(currencyTransactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _currencyTransactionService.DeleteAsync(id);
    }
}
