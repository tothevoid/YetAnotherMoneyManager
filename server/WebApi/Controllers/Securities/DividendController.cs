using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Models.Common;
using MoneyManager.WebApi.Models.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<DividendModel>> GetAll(GetAllDividendsQuery query)
        {
            var securities = await _dividendService.GetAll(query.SecurityId, query.PageIndex, query.RecordsQuantity);
            return _mapper.Map<IEnumerable<DividendModel>>(securities);
        }

        [HttpGet(nameof(GetAvailable))]
        public async Task<IEnumerable<DividendModel>> GetAvailable(Guid brokerAccountId)
        {
            var securities = await _dividendService.GetAvailable(brokerAccountId);
            return _mapper.Map<IEnumerable<DividendModel>>(securities);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination([FromQuery] Guid securityId)
        {
            var pagination = await _dividendService.GetPagination(securityId);
            return _mapper.Map<PaginationConfigModel>(pagination);
        }

        [HttpPut]
        public async Task<Guid> Add(DividendModel dividend)
        {
            var dividendDto = _mapper.Map<DividendDto>(dividend);
            return await _dividendService.Add(dividendDto);
        }

        [HttpPatch]
        public async Task Update(DividendModel dividend)
        {
            var dividendDto = _mapper.Map<DividendDto>(dividend);
            await _dividendService.Update(dividendDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _dividendService.Delete(id);
    }
}