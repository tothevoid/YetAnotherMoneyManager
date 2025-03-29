using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using BLL.Interfaces.Entities;
using MoneyManager.BLL.DTO;
using MoneyManager.Model.Charts.Deposit;
using MoneyManager.Model.Common;
using MoneyManager.Model.Server;
using MoneyManager.WEB.Model;

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
            var deposits = await _depositService.GetAll();
            return _mapper.Map<IEnumerable<DepositModel>>(deposits);
        }

        [HttpGet(nameof(GetDepositsSummary))]
        public async Task<DepositMonthSummary> GetDepositsSummary()
        {
            var summary = await _depositService.GetSummary();
            return _mapper.Map<DepositMonthSummary>(summary);
        }

        [HttpPut]
        public async Task<Guid> Add(DepositModel deposit)
        {
            var depositDto = _mapper.Map<DepositDTO>(deposit);
            return await _depositService.Add(depositDto);
        }

        [HttpPatch]
        public async Task Update(DepositModel modifiedDeposit)
        {
            var deposit = _mapper.Map<DepositDTO>(modifiedDeposit);
            await _depositService.Update(deposit);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _depositService.Delete(id);
        }

        [HttpGet(nameof(GetDepositsRange))]
        public async Task<DepositsRangeModel> GetDepositsRange()
        {
            var rangeDto = await _depositService.GetDepositsRange();
            return _mapper.Map<DepositsRangeModel>(rangeDto);
        }
    }
}