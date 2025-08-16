using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.Application.Interfaces.Securities;

namespace MoneyManager.WebApi.Controllers.Securities
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class DividendController : ControllerBase
    {
        private readonly IDividendService _dividendService;
        private readonly IMapper _mapper;
        public DividendController(IDividendService dividendService, IMapper mapper)
        {
            _mapper = mapper;
            _dividendService = dividendService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IEnumerable<DividendModel>> GetAll(Guid securityId)
        {
            var securities = await _dividendService.GetAll(securityId);
            return _mapper.Map<IEnumerable<DividendModel>>(securities);
        }

        [HttpGet(nameof(GetAvailable))]
        public async Task<IEnumerable<DividendModel>> GetAvailable(Guid brokerAccountId)
        {
            var securities = await _dividendService.GetAvailable(brokerAccountId);
            return _mapper.Map<IEnumerable<DividendModel>>(securities);
        }

        [HttpPut]
        public async Task<Guid> Add(DividendModel security)
        {
            var dividendDto = _mapper.Map<DividendDto>(security);
            return await _dividendService.Add(dividendDto);
        }

        [HttpPatch]
        public async Task Update(DividendModel security)
        {
            var dividendDto = _mapper.Map<DividendDto>(security);
            await _dividendService.Update(dividendDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _dividendService.Delete(id);
    }
}