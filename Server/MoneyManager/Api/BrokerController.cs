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
    public class BrokerController : ControllerBase
    {
        private readonly IBrokerService _brokerService;
        private readonly IMapper _mapper;
        public BrokerController(IBrokerService brokerService, IMapper mapper)
        {
            _mapper = mapper;
            _brokerService = brokerService;
        }

        [HttpPost("GetAll")]
        public IEnumerable<BrokerModel> GetAll()
        {
            var securities = _brokerService.GetAll();
            return _mapper.Map<IEnumerable<BrokerModel>>(securities);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerModel broker)
        {
            var brokerDto = _mapper.Map<BrokerDto>(broker);
            return await _brokerService.Add(brokerDto);
        }

        [HttpPatch]
        public async Task Update(BrokerModel broker)
        {
            var brokerDto = _mapper.Map<BrokerDto>(broker);
            await _brokerService.Update(brokerDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerService.Delete(id);
    }
}