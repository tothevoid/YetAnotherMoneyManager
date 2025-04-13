using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.WebApi.Models.Deposits;

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
        public IEnumerable<ServerDepositModel> GetAll(DepositFiltrationModel filtration)
        {
            var deposits = _depositService.GetAll(filtration.MonthsFrom, filtration.MonthsTo, filtration.OnlyActive);
            return _mapper.Map<IEnumerable<ServerDepositModel>>(deposits);
        }

        [HttpPost(nameof(GetDepositsSummary))]
        public DepositMonthSummary GetDepositsSummary(DepositFiltrationModel filtration)
        {
            var summary = _depositService.GetSummary(filtration.MonthsFrom, filtration.MonthsTo, filtration.OnlyActive);
            return _mapper.Map<DepositMonthSummary>(summary);
        }

        [HttpPut]
        public async Task<Guid> Add(ClientDepositModel deposit)
        {
            var depositDto = _mapper.Map<ClientDepositDto>(deposit);
            return await _depositService.Add(depositDto);
        }

        [HttpPatch]
        public async Task Update(ClientDepositModel modifiedDeposit)
        {
            var deposit = _mapper.Map<ClientDepositDto>(modifiedDeposit);
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