﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.WebApi.Models.Deposits;
using MoneyManager.WebApi.Models.Deposits.Charts;
using MoneyManager.Application.DTO.Deposits;

namespace MoneyManager.WebApi.Controllers.Deposits
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

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<DepositModel>> GetAll(DepositFiltrationModel filtration)
        {
            var deposits = await _depositService.GetAll(filtration.MonthsFrom, 
                filtration.MonthsTo, filtration.OnlyActive);
            return _mapper.Map<IEnumerable<DepositModel>>(deposits);
        }

        [HttpPost(nameof(GetDepositsSummary))]
        public async Task<DepositMonthSummary> GetDepositsSummary(DepositFiltrationModel filtration)
        {
            var summary = await _depositService.GetSummary(filtration.MonthsFrom, filtration.MonthsTo, filtration.OnlyActive);
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