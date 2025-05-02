using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;

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

        [HttpPost("GetAll")]
        public async Task<IEnumerable<BrokerAccountSecurityModel>> GetAll(BrokerAccountSecurityRequestModel request)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityService
                .GetAll(request.BrokerAccountId, request.RecordsQuantity, request.PageIndex);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityModel>>(brokerAccountSecurities);
        }

        [HttpGet(nameof(GetByBrokerAccount))]
        public async Task<IEnumerable<BrokerAccountSecurityModel>> GetByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityService
                .GetByBrokerAccount(brokerAccountId);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityModel>>(brokerAccountSecurities);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<BrokerAccountSecurityPaginationModel> GetPagination([FromQuery] Guid brokerAccountId)
        {
            var pagination = await _brokerAccountSecurityService
                .GetPagination(brokerAccountId);
            return _mapper.Map<BrokerAccountSecurityPaginationModel>(pagination);
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