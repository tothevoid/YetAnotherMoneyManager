using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.WebApi.Models.Debts;

namespace MoneyManager.WebApi.Controllers.Debts
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class DebtController : ControllerBase
    {
        private readonly IDebtService _debtService;
        private readonly IMapper _mapper;
        public DebtController(IDebtService debtService, IMapper mapper)
        {
            _mapper = mapper;
            _debtService = debtService;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<DebtModel>> GetAll([FromQuery] bool onlyActive)
        {
            var debts = await _debtService.GetAll(onlyActive);
            return _mapper.Map<IEnumerable<DebtModel>>(debts);
        }

        [HttpPut]
        public async Task<Guid> Add(DebtModel debt)
        {
            var debtDto = _mapper.Map<DebtDto>(debt);
            return await _debtService.Add(debtDto);
        }

        [HttpPatch]
        public async Task Update(DebtModel debt)
        {
            var debtDto = _mapper.Map<DebtDto>(debt);
            await _debtService.Update(debtDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _debtService.Delete(id);
    }
}
