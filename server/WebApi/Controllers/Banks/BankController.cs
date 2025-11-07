using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.Interfaces.Banks;
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
        private readonly IMapper _mapper;
        public BankController(IBankService bankService, IMapper mapper)
        {
            _mapper = mapper;
            _bankService = bankService;
        }

        [HttpGet]
        public async Task<IEnumerable<BankModel>> GetAll()
        {
            var banks = await _bankService.GetAll();
            return _mapper.Map<IEnumerable<BankModel>>(banks);
        }

        [HttpGet(nameof(GetById))]
        public async Task<BankModel> GetById([FromQuery] Guid id)
        {
            var bank = await _bankService.GetById(id);
            return _mapper.Map<BankModel>(bank);
        }

        [HttpPut]
        public async Task<BankModel> Add([FromForm] string bankJson, [FromForm] IFormFile bankIcon = null)
        {
            var bank = JsonSerializer.Deserialize<BankModel>(bankJson);

            var bankDto = _mapper.Map<BankDto>(bank);
            var createdBank = await _bankService.Add(bankDto, bankIcon);
            return _mapper.Map<BankModel>(createdBank);
        }

        [HttpPatch]
        public async Task<BankModel> Update([FromForm] string bankJson, [FromForm] IFormFile bankIcon = null)
        {
            var bank = JsonSerializer.Deserialize<BankModel>(bankJson);

            var bankDto = _mapper.Map<BankDto>(bank);
            var updatedBank = await _bankService.Update(bankDto, bankIcon);
            return _mapper.Map<BankModel>(updatedBank);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _bankService.Delete(id);

        [HttpGet("icon")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBankIcon(string iconKey)
        {
            var url = await _bankService.GetIconUrl(iconKey);
            return Redirect(url);
        }
    }
}