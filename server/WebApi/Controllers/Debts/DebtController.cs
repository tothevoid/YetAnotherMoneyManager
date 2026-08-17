using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Debts;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Debts
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DebtController : ControllerBase
    {
        private readonly IDebtService _debtService;
        private readonly WebApiMapper _mapper;
        public DebtController(IDebtService debtService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _debtService = debtService;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<DebtModel>> GetAll([FromQuery] bool onlyActive)
        {
            var debts = await _debtService.GetAllAsync(onlyActive);
            return _mapper.Map(debts);
        }

        [HttpPut]
        public async Task<Guid> Add(DebtModel debt)
        {
            var debtDto = _mapper.Map(debt);
            return await _debtService.AddAsync(debtDto);
        }

        [HttpPatch]
        public async Task Update(DebtModel debt)
        {
            var debtDto = _mapper.Map(debt);
            await _debtService.UpdateAsync(debtDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _debtService.DeleteAsync(id);
    }
}
