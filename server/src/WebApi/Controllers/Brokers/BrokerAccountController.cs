using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Audex.Application.DTO.Brokers;
using Audex.Application.Interfaces.Brokers;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Brokers;
using Microsoft.AspNetCore.Authorization;

namespace Audex.WebApi.Controllers.Brokers
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
            var brokerAccounts = await _brokerAccountService.GetAllAsync();
            return _mapper.Map(brokerAccounts);
        }

        [HttpGet("GetById")]
        public async Task<BrokerAccountModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _brokerAccountService.GetByIdAsync(id);
            return _mapper.Map(brokerAccount);
        }

        [HttpGet(nameof(GetTotalSoldAmountByBrokerAccount))]
        public async Task<decimal> GetTotalSoldAmountByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            return await _brokerAccountService.GetTotalSoldAmountByBrokerAccountAsync(brokerAccountId);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map(brokerAccount);
            return await _brokerAccountService.AddAsync(brokerAccountDto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map(brokerAccount);
            await _brokerAccountService.UpdateAsync(brokerAccountDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountService.DeleteAsync(id);
    }
}