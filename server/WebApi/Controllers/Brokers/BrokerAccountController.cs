using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Brokers;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BrokerAccountController : ControllerBase
    {
        private readonly IBrokerAccountSummaryService _brokerAccountSummaryService;
        private readonly IBrokerAccountService _brokerAccountService;

        private readonly WebApiMapper _mapper;
        public BrokerAccountController(IBrokerAccountService brokerAccountService, 
            IBrokerAccountSummaryService brokerAccountSummaryService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _brokerAccountService = brokerAccountService;
            _brokerAccountSummaryService = brokerAccountSummaryService;
        }

        [HttpGet]
        public async Task<IEnumerable<BrokerAccountModel>> GetAll()
        {
            var brokerAccounts = await _brokerAccountService.GetAll();
            return _mapper.Map(brokerAccounts);
        }

        [HttpGet("GetById")]
        public async Task<BrokerAccountModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _brokerAccountService.GetById(id);
            return _mapper.Map(brokerAccount);
        }

        [HttpGet(nameof(GetTotalSoldAmountByBrokerAccountId))]
        public async Task<decimal> GetTotalSoldAmountByBrokerAccountId([FromQuery] Guid brokerAccountId)
        {
            return await _brokerAccountService.GetTotalSoldAmountByBrokerAccountId(brokerAccountId);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map(brokerAccount);
            return await _brokerAccountService.Add(brokerAccountDto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map(brokerAccount);
            await _brokerAccountService.Update(brokerAccountDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountService.Delete(id);
    }
}