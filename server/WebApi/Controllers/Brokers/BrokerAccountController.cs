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
    public class BrokerAccountController : ControllerBase
    {
        private readonly IBrokerAccountService _brokerAccountService;
        private readonly IMapper _mapper;
        public BrokerAccountController(IBrokerAccountService brokerAccountService, IMapper mapper)
        {
            _mapper = mapper;
            _brokerAccountService = brokerAccountService;
        }

        [HttpGet]
        public async Task<IEnumerable<BrokerAccountModel>> GetAll()
        {
            var brokerAccounts = await _brokerAccountService.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountModel>>(brokerAccounts);
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