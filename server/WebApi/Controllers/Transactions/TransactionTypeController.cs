using System;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Transactions;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Transactions;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Transactions
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly WebApiMapper _mapper;
        public TransactionTypeController(ITransactionTypeService transactionTypeService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _transactionTypeService = transactionTypeService;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionTypeModel>> GetAll(bool onlyActive = false)
        {
            var transactions = await _transactionTypeService.GetAllAsync(onlyActive);
            return _mapper.Map(transactions);
        }

        [HttpPut]
        public async Task<TransactionTypeModel> Add([FromForm] string transactionTypeJson, [FromForm] IFormFile transactionTypeIcon)
        {
            var transactionType = JsonSerializer.Deserialize<TransactionTypeModel>(transactionTypeJson);
            var transactionTypeDto = _mapper.Map(transactionType);
            var transactionTypeResult = await _transactionTypeService.AddAsync(transactionTypeDto, transactionTypeIcon);

            return _mapper.Map(transactionTypeResult);
        }

        [HttpPatch]
        public async Task<TransactionTypeModel> Update([FromForm] string transactionTypeJson, [FromForm] IFormFile transactionTypeIcon = null)
        {
            var transactionType = JsonSerializer.Deserialize<TransactionTypeModel>(transactionTypeJson);
            var transactionTypeDto = _mapper.Map(transactionType);
            var transactionTypeResult = await _transactionTypeService.UpdateAsync(transactionTypeDto, transactionTypeIcon);

            return _mapper.Map(transactionTypeResult);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _transactionTypeService.DeleteAsync(id);

        [HttpGet("icon")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSecurityIcon(string iconKey)
        {
            Response.Headers.CacheControl = "public, max-age=31536000, immutable";
            var url = await _transactionTypeService.GetIconUrlAsync(iconKey);
            return Redirect(url);
        }
    }
}