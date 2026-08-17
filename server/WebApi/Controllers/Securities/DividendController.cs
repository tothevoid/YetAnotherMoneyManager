using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Mappings;
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
    [Authorize]
    public class DividendController : ControllerBase
    {
        private readonly IDividendService _dividendService;
        private readonly WebApiMapper _mapper;
        public DividendController(IDividendService dividendService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _dividendService = dividendService;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<DividendModel>> GetAll(GetAllDividendsQuery query)
        {
            var securities = await _dividendService.GetAllAsync(query.SecurityId, query.PageIndex, query.RecordsQuantity);
            return _mapper.Map(securities);
        }

        [HttpGet(nameof(GetAvailable))]
        public async Task<IEnumerable<DividendModel>> GetAvailable(Guid brokerAccountId)
        {
            var securities = await _dividendService.GetAvailableAsync(brokerAccountId);
            return _mapper.Map(securities);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination([FromQuery] Guid securityId)
        {
            var pagination = await _dividendService.GetPaginationAsync(securityId);
            return _mapper.Map(pagination);
        }

        [HttpPut]
        public async Task<Guid> Add(DividendModel dividend)
        {
            var dividendDto = _mapper.Map(dividend);
            return await _dividendService.AddAsync(dividendDto);
        }

        [HttpPatch]
        public async Task Update(DividendModel dividend)
        {
            var dividendDto = _mapper.Map(dividend);
            await _dividendService.UpdateAsync(dividendDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _dividendService.DeleteAsync(id);
    }
}