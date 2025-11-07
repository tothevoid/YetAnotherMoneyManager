using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
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

        private readonly IMapper _mapper;
        public BrokerAccountController(IBrokerAccountService brokerAccountService, 
            IBrokerAccountSummaryService brokerAccountSummaryService, IMapper mapper)
        {
            _mapper = mapper;
            _brokerAccountService = brokerAccountService;
            _brokerAccountSummaryService = brokerAccountSummaryService;
        }

        [HttpGet]
        public async Task<IEnumerable<BrokerAccountModel>> GetAll()
        {
            var brokerAccounts = await _brokerAccountService.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountModel>>(brokerAccounts);
        }

        [HttpGet("GetSummary")]
        public async Task<BrokerAccountSummaryModel> GetSummary(Guid brokerAccountId, DateTime from, DateTime to)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetSummary(brokerAccountId, from, to);
            return _mapper.Map<BrokerAccountSummaryModel>(brokerAccount);
        }

        [HttpGet("GetById")]
        public async Task<BrokerAccountModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _brokerAccountService.GetById(id);
            return _mapper.Map<BrokerAccountModel>(brokerAccount);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map<BrokerAccountDTO>(brokerAccount);
            return await _brokerAccountService.Add(brokerAccountDto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map<BrokerAccountDTO>(brokerAccount);
            await _brokerAccountService.Update(brokerAccountDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountService.Delete(id);
    }
}