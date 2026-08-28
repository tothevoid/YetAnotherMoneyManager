using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Services.Brokers;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Brokers;
using MoneyManager.WebApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DividendPaymentController : ControllerBase
    {
        private readonly IDividendPaymentService _dividendPaymentService;
        private readonly WebApiMapper _mapper;

        public DividendPaymentController(IDividendPaymentService dividendPaymentService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _dividendPaymentService = dividendPaymentService;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<DividendPaymentModel>> GetAll(GetAllDividendsPaymentsQuery query)
        {
            var dividendPayments = await _dividendPaymentService.GetAllAsync(query.BrokerAccountId, query.PageIndex, query.RecordsQuantity);
            return _mapper.Map(dividendPayments);
        }

        [HttpGet(nameof(GetEarningsByBrokerAccount))]
        public async Task<decimal> GetEarningsByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            return await _dividendPaymentService.GetEarningsByBrokerAccountAsync(brokerAccountId);
        }

        [HttpPut]
        public async Task<Guid> Add(DividendPaymentModel dividendPayment)
        {
            var dividendDto = _mapper.Map(dividendPayment);
            return await _dividendPaymentService.AddAsync(dividendDto);
        }

        [HttpPatch]
        public async Task Update(DividendPaymentModel dividendPayment)
        {
            var dividendDto = _mapper.Map(dividendPayment);
            await _dividendPaymentService.UpdateAsync(dividendDto);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination()
        {
            var pagination = await _dividendPaymentService
                .GetPaginationAsync();
            return _mapper.Map(pagination);
        }

        [HttpGet(nameof(GetPaginationByBrokerAccount))]
        public async Task<PaginationConfigModel> GetPaginationByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var pagination = await _dividendPaymentService
                .GetPaginationByBrokerAccountAsync(brokerAccountId);
            return _mapper.Map(pagination);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _dividendPaymentService.DeleteAsync(id);
    }
}