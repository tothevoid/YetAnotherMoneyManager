using Microsoft.AspNetCore.Mvc;
using Audex.Application.Interfaces.Brokers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using Audex.Application.DTO.Brokers;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Brokers;

namespace Audex.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BrokerAccountTaxDeductionController : ControllerBase
    {
        private readonly IBrokerAccountTaxDeductionService _service;
        private readonly WebApiMapper _mapper;

        public BrokerAccountTaxDeductionController(IBrokerAccountTaxDeductionService service, WebApiMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<BrokerAccountTaxDeductionModel>> GetAll(GetAllBrokerAccountsTaxDeductionsQuery query)
        {
            var dtos = await _service.GetAllAsync(query.BrokerAccountId);
            return _mapper.Map(dtos);
        }

        [HttpPut]
        public async Task<Guid> Add(BrokerAccountTaxDeductionModel model)
        {
            var dto = _mapper.Map(model);
            return await _service.AddAsync(dto);
        }

        [HttpGet(nameof(GetAmountByBrokerAccount))]
        public async Task<decimal> GetAmountByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            return await _service.GetAmountByBrokerAccountAsync(brokerAccountId);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountTaxDeductionModel model)
        {
            var dto = _mapper.Map(model);
            await _service.UpdateAsync(dto);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _service.DeleteAsync(id);
        }
    }
}