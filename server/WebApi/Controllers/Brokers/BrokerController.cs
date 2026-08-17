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
    public class BrokerController : ControllerBase
    {
        private readonly IBrokerService _brokerService;
        private readonly WebApiMapper _mapper;
        public BrokerController(IBrokerService brokerService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _brokerService = brokerService;
        }

        [HttpGet]
        public async Task<IEnumerable<BrokerModel>> GetAll()
        {
            var securities = await _brokerService.GetAllAsync();
            return _mapper.Map(securities);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerModel broker)
        {
            var brokerDto = _mapper.Map(broker);
            return await _brokerService.AddAsync(brokerDto);
        }

        [HttpPatch]
        public async Task Update(BrokerModel broker)
        {
            var brokerDto = _mapper.Map(broker);
            await _brokerService.UpdateAsync(brokerDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerService.DeleteAsync(id);
    }
}