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
    public class BrokerAccountTypeController : ControllerBase
    {
        private readonly IBrokerAccountTypeService _brokerAccountTypeService;
        private readonly IMapper _mapper;
        public BrokerAccountTypeController(IBrokerAccountTypeService brokerAccountTypeService, IMapper mapper)
        {
            _mapper = mapper;
            _brokerAccountTypeService = brokerAccountTypeService;
        }

        [HttpPost("GetAll")]
        public IEnumerable<BrokerAccountTypeModel> GetAll()
        {
            var brokerAccountsTypes = _brokerAccountTypeService.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountTypeModel>>(brokerAccountsTypes);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountTypeModel brokerAccountType)
        {
            var brokerAccountTypeDto = _mapper.Map<BrokerAccountTypeDto>(brokerAccountType);
            return await _brokerAccountTypeService.Add(brokerAccountTypeDto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountTypeModel brokerAccountType)
        {
            var brokerAccountTypeDto = _mapper.Map<BrokerAccountTypeDto>(brokerAccountType);
            await _brokerAccountTypeService.Update(brokerAccountTypeDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountTypeService.Delete(id);
    }
}