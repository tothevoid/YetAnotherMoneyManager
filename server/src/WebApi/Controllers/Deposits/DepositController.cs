using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Audex.Application.DTO.Deposits;
using Audex.Application.Interfaces.Deposits;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Deposits;
using Audex.WebApi.Models.Deposits.Charts;
using Microsoft.AspNetCore.Authorization;

namespace Audex.WebApi.Controllers.Deposits
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;
        private readonly WebApiMapper _mapper;
        public DepositController(IDepositService depositService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _depositService = depositService;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<DepositModel>> GetAll(DepositFiltrationModel filtration)
        {
            var deposits = await _depositService.GetAllAsync(filtration.MonthsFrom, 
                filtration.MonthsTo, filtration.OnlyActive);
            return _mapper.Map(deposits);
        }

        [HttpPost(nameof(GetDepositsSummary))]
        public async Task<DepositMonthSummary> GetDepositsSummary(DepositFiltrationModel filtration)
        {
            var summary = await _depositService.GetSummaryAsync(filtration.MonthsFrom, filtration.MonthsTo, filtration.OnlyActive);
            return _mapper.Map(summary);
        }

        [HttpPut]
        public async Task<Guid> Add(DepositModel deposit)
        {
            var depositDto = _mapper.Map(deposit);
            return await _depositService.AddAsync(depositDto);
        }

        [HttpPatch]
        public async Task Update(DepositModel modifiedDeposit)
        {
            var deposit = _mapper.Map(modifiedDeposit);
            await _depositService.UpdateAsync(deposit);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _depositService.DeleteAsync(id);
        }

        [HttpGet(nameof(GetDepositsRange))]
        public async Task<DepositsRangeModel> GetDepositsRange()
        {
            var rangeDto = await _depositService.GetDepositsRangeAsync();
            return _mapper.Map(rangeDto);
        }
    }
}