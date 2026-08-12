using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Debts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Controllers.Debts
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DebtTagController : ControllerBase
    {
        private readonly IDebtTagService _debtTagService;
        private readonly WebApiMapper _mapper;

        public DebtTagController(IDebtTagService debtTagService, WebApiMapper mapper)
        {
            _debtTagService = debtTagService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<DebtTagModel>> GetAll()
        {
            var tags = await _debtTagService.GetAll();
            return _mapper.Map(tags);
        }

        [HttpGet("stats")]
        public async Task<IEnumerable<DebtTagStatsModel>> GetStats()
        {
            var stats = await _debtTagService.GetStats();
            return _mapper.Map(stats);
        }

        [HttpPut]
        public async Task<Guid> Add([FromBody] DebtTagModel model)
        {
            var dto = _mapper.Map(model);
            return await _debtTagService.Add(dto);
        }

        [HttpPatch]
        public async Task Update([FromBody] DebtTagModel model)
        {
            var dto = _mapper.Map(model);
            await _debtTagService.Update(dto);
        }

        [HttpDelete]
        public async Task Delete([FromQuery] Guid id)
        {
            await _debtTagService.Delete(id);
        }

        [HttpPut("debt/{debtId}")]
        public async Task AssignTagsToDebt([FromRoute] Guid debtId, [FromBody] IEnumerable<Guid> tagIds)
        {
            await _debtTagService.AssignTagsToDebt(debtId, tagIds);
        }
    }
}
