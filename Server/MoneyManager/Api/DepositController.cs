using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using BLL.Interfaces.Entities;
using MoneyManager.BLL.DTO;
using MoneyManager.Model.Common;

namespace MoneyManager.WEB.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;
        private readonly IMapper _mapper;
        public DepositController(IDepositService depositService, IMapper mapper)
        {
            _mapper = mapper;
            _depositService = depositService;
        }

        [HttpGet]
        public async Task<IEnumerable<DepositModel>> GetAll()
        {
            var transactions = await _depositService.GetAll();
            return _mapper.Map<IEnumerable<DepositModel>>(transactions);
        }

        [HttpPut]
        public async Task<Guid> Add(DepositModel transaction)
        {
            var transactionDTO = _mapper.Map<DepositDTO>(transaction);
            return await _depositService.Add(transactionDTO);
        }

        [HttpDelete]
        public async Task Delete(DepositModel transaction)
        {
            var transactionDTO = _mapper.Map<DepositDTO>(transaction);
            await _depositService.Delete(transactionDTO);
        }

        [HttpPatch]
        public async Task Update(DepositModel modifiedDeposit)
        {
            var deposit = _mapper.Map<DepositDTO>(modifiedDeposit);
            await _depositService.Update(deposit);
        }
    }
}