using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Brokers;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.WebApi.Models.Brokers;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BrokerAccountTaxDeductionController : ControllerBase
    {
        private readonly IBrokerAccountTaxDeductionService _service;
        private readonly IMapper _mapper;

        public BrokerAccountTaxDeductionController(IBrokerAccountTaxDeductionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BrokerAccountTaxDeductionModel>> GetAll()
        {
            var dtos = await _service.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountTaxDeductionModel>>(dtos);
        }

        [HttpPost]
        public async Task<Guid> Add(BrokerAccountTaxDeductionModel model)
        {
            var dto = _mapper.Map<BrokerAccountTaxDeductionDto>(model);
            return await _service.Add(dto);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountTaxDeductionModel model)
        {
            var dto = _mapper.Map<BrokerAccountTaxDeductionDto>(model);
            await _service.Update(dto);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _service.Delete(id);
        }
    }
}