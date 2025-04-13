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
            var brokerDto = _mapper.Map<BrokerDTO>(broker);
            return await _brokerService.Add(brokerDto);
        }

        [HttpPatch]
        public async Task Update(BrokerModel broker)
        {
            var brokerDto = _mapper.Map<BrokerDTO>(broker);
            await _brokerService.Update(brokerDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerService.Delete(id);
    }
}