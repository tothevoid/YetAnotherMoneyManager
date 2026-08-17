using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Banks;
using MoneyManager.WebApi.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Controllers.Banks
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly WebApiMapper _mapper;
        public BankController(IBankService bankService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _bankService = bankService;
        }

        [HttpGet]
        public async Task<IEnumerable<BankModel>> GetAll()
        {
            var banks = await _bankService.GetAllAsync();
            return _mapper.Map(banks);
        }

        [HttpGet(nameof(GetById))]
        public async Task<BankModel> GetById([FromQuery] Guid id)
        {
            var bank = await _bankService.GetByIdAsync(id);
            return _mapper.Map(bank);
        }

        [HttpPut]
        public async Task<BankModel> Add([FromForm] string bankJson, [FromForm] IFormFile bankIcon = null)
        {
            var bank = JsonSerializer.Deserialize<BankModel>(bankJson);

            var bankDto = _mapper.Map(bank);
            var createdBank = await _bankService.AddAsync(bankDto, bankIcon);
            return _mapper.Map(createdBank);
        }

        [HttpPatch]
        public async Task<BankModel> Update([FromForm] string bankJson, [FromForm] IFormFile bankIcon = null)
        {
            var bank = JsonSerializer.Deserialize<BankModel>(bankJson);

            var bankDto = _mapper.Map(bank);
            var updatedBank = await _bankService.UpdateAsync(bankDto, bankIcon);
            return _mapper.Map(updatedBank);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _bankService.DeleteAsync(id);

        [HttpGet("icon")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBankIcon(string iconKey)
        {
            Response.Headers.CacheControl = "public, max-age=31536000, immutable";
            var url = await _bankService.GetIconUrlAsync(iconKey);
            return Redirect(url);
        }
    }
}