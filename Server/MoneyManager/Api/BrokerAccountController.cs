using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.WEB.Model;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;

namespace MoneyManager.WEB.Api
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

        [HttpPost("GetAll")]
        public IEnumerable<BrokerAccountModel> GetAll()
        {
            var brokerAccounts = _brokerAccountService.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountModel>>(brokerAccounts);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map<BrokerAccountDto>(brokerAccount);
            brokerAccountDto.CurrencyId = brokerAccountDto.Currency.Id;
            brokerAccountDto.TypeId = brokerAccountDto.Type.Id;
            brokerAccountDto.BrokerId = brokerAccountDto.Broker.Id;
            return await _brokerAccountService.Add(brokerAccountDto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountTypeModel brokerAccount)
        {
            var brokerAccountDto = _mapper.Map<BrokerAccountDto>(brokerAccount);
            await _brokerAccountService.Update(brokerAccountDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountService.Delete(id);
    }
}