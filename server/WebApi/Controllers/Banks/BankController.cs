using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.WebApi.Models.Banks;

namespace MoneyManager.WebApi.Controllers.Banks
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
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

        [HttpGet("GetById")]
        public async Task<BankModel> GetById([FromQuery] Guid id)
        {
            var bank = await _bankService.GetById(id);
            return _mapper.Map<BankModel>(bank);
        }

        [HttpPut]
        public async Task<Guid> Add(BankModel bank)
        {
            var bankDto = _mapper.Map<BankDto>(bank);
            var created = await _bankService.Add(bankDto);
            return created.Id;
        }

        [HttpPatch]
        public async Task Update(BankModel bank)
        {
            var bankDto = _mapper.Map<BankDto>(bank);
            await _bankService.Update(bankDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _bankService.Delete(id);
    }
}