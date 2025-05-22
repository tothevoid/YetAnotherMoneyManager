using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Transactions;
using System.Collections.Generic;
using System.Text.Json;
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
        public async Task<IEnumerable<TransactionTypeModel>> GetAll(bool onlyActive = false)
        {
            var transactions = await _transactionTypeService.GetAll(onlyActive);
            return _mapper.Map<IEnumerable<TransactionTypeModel>>(transactions);
        }

        [HttpPut]
        public async Task<TransactionTypeModel> Add([FromForm] string transactionTypeJson, [FromForm] IFormFile? transactionTypeIcon)
        {
            var transactionType = JsonSerializer.Deserialize<TransactionTypeModel>(transactionTypeJson);
            var transactionTypeDto = _mapper.Map<TransactionTypeDTO>(transactionType);
            var transactionTypeResult = await _transactionTypeService.Add(transactionTypeDto, transactionTypeIcon);

            return _mapper.Map<TransactionTypeModel>(transactionTypeResult);
        }

        [HttpPatch]
        public async Task<TransactionTypeModel> Update([FromForm] string transactionTypeJson, [FromForm] IFormFile? transactionTypeIcon)
        {
            var transactionType = JsonSerializer.Deserialize<TransactionTypeModel>(transactionTypeJson);
            var transactionTypeDto = _mapper.Map<TransactionTypeDTO>(transactionType);
            var transactionTypeResult = await _transactionTypeService.Update(transactionTypeDto, transactionTypeIcon);

            return _mapper.Map<TransactionTypeModel>(transactionTypeResult);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _transactionTypeService.Delete(id);

        [HttpGet("icon")]
        public async Task<IActionResult> GetSecurityIcon(string iconKey)
        {
            var url = await _transactionTypeService.GetIconUrl(iconKey);
            return Redirect(url);
        }
    }
}