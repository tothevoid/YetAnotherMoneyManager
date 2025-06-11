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
    public class DividendPaymentController : ControllerBase
    {
        private readonly IDividendPaymentService _dividendPaymentService;
        private readonly IMapper _mapper;

        public DividendPaymentController(IDividendPaymentService dividendPaymentService, IMapper mapper)
        {
            _mapper = mapper;
            _dividendPaymentService = dividendPaymentService;
        }

        [HttpGet(nameof(GetByBrokerAccount))]
        public async Task<IEnumerable<DividendPaymentModel>> GetByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var dividendPayments = await _dividendPaymentService.GetAll(brokerAccountId);
            return _mapper.Map<IEnumerable<DividendPaymentModel>>(dividendPayments);
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

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _dividendPaymentService.Delete(id);
    }
}