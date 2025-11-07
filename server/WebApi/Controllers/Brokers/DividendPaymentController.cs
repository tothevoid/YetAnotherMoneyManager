using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Services.Brokers;
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
        private readonly IMapper _mapper;

        public DividendPaymentController(IDividendPaymentService dividendPaymentService, IMapper mapper)
        {
            _mapper = mapper;
            _dividendPaymentService = dividendPaymentService;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<DividendPaymentModel>> GetAll(GetAllDividendsPaymentsQuery query)
        {
            var dividendPayments = await _dividendPaymentService.GetAll(query.BrokerAccountId, query.PageIndex, query.RecordsQuantity);
            return _mapper.Map<IEnumerable<DividendPaymentModel>>(dividendPayments);
        }

        [HttpGet(nameof(GetEarningsByBrokerAccount))]
        public async Task<decimal> GetEarningsByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            return await _dividendPaymentService.GetEarningsByBrokerAccount(brokerAccountId);
        }

        [HttpPut]
        public async Task<Guid> Add(DividendPaymentModel dividendPayment)
        {
            var dividendDto = _mapper.Map<DividendPaymentDto>(dividendPayment);
            return await _dividendPaymentService.Add(dividendDto);
        }

        [HttpPatch]
        public async Task Update(DividendPaymentModel dividendPayment)
        {
            var dividendDto = _mapper.Map<DividendPaymentDto>(dividendPayment);
            await _dividendPaymentService.Update(dividendDto);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination([FromQuery] Guid brokerAccountId)
        {
            var pagination = await _dividendPaymentService
                .GetPagination(brokerAccountId);
            return _mapper.Map<PaginationConfigModel>(pagination);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _dividendPaymentService.Delete(id);
    }
}