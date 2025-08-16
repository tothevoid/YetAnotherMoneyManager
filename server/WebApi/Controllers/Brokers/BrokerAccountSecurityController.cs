using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.Application.Services.Brokers;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class BrokerAccountSecurityController : ControllerBase
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;
        private readonly IMapper _mapper;
        public BrokerAccountSecurityController(IBrokerAccountSecurityService brokerAccountSecurityServiceRepo,
            IMapper mapper)
        {
            _mapper = mapper;
            _brokerAccountSecurityService = brokerAccountSecurityServiceRepo;
        }

        [HttpGet(nameof(GetByBrokerAccount))]
        public async Task<IEnumerable<BrokerAccountSecurityModel>> GetByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityService
                .GetByBrokerAccount(brokerAccountId);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityModel>>(brokerAccountSecurities);
        }

        [HttpGet(nameof(PullQuotations))]
        public async Task PullQuotations([FromQuery] Guid brokerAccountId)
        {
            await _brokerAccountSecurityService.PullQuotations(brokerAccountId);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountSecurityModel brokerAccountSecurity)
        {
            var brokerAccountSecurityDto = _mapper.Map<BrokerAccountSecurityDTO>(brokerAccountSecurity);
            return await _brokerAccountSecurityService.Add(brokerAccountSecurityDto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountSecurityModel brokerAccountSecurity)
        {
            var brokerAccountSecurityDto = _mapper.Map<BrokerAccountSecurityDTO>(brokerAccountSecurity);
            await _brokerAccountSecurityService.Update(brokerAccountSecurityDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountSecurityService.Delete(id);
    }
}