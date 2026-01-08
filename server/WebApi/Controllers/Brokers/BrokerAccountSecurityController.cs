using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Models.Brokers;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.Application.Services.Brokers;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BrokerAccountSecurityController : ControllerBase
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;
        private readonly IMapper _mapper;
        private readonly IPullQuotationsService _pullQuotationsService;

        public BrokerAccountSecurityController(IBrokerAccountSecurityService brokerAccountSecurityServiceRepo,
            IPullQuotationsService pullQuotationsService,
            IMapper mapper)
        {
            _mapper = mapper;
            _brokerAccountSecurityService = brokerAccountSecurityServiceRepo;
            _pullQuotationsService = pullQuotationsService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IEnumerable<BrokerAccountSecurityModel>> GetAll()
        {
            var brokerAccountSecurities = await _brokerAccountSecurityService
                .GetAll(true);

            return _mapper.Map<IEnumerable<BrokerAccountSecurityModel>>(brokerAccountSecurities);
        }

        [HttpGet(nameof(GetByBrokerAccount))]
        public async Task<IEnumerable<BrokerAccountSecurityModel>> GetByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityService
                .GetByBrokerAccount(brokerAccountId);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityModel>>(brokerAccountSecurities);
        }

        [HttpGet(nameof(PullQuotations))]
        public async Task PullQuotations()
        {
            await _brokerAccountSecurityService.PullQuotations();
        }

        [HttpGet(nameof(PullQuotationsByBrokerAccount))]
        public async Task PullQuotationsByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            await _brokerAccountSecurityService.PullQuotationsByBrokerAccount(brokerAccountId);
        }

        [HttpGet(nameof(GetLastPullDate))]
        public LastPullInfoModel GetLastPullDate()
        {
            return new LastPullInfoModel() { LastPullDate = _pullQuotationsService.LastPullDate ?? DateTime.UtcNow};
        }
    }
}