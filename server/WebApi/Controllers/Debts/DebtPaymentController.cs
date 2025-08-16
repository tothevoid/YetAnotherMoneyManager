using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.WebApi.Models.Debts;

namespace MoneyManager.WebApi.Controllers.Debts
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class DebtPaymentController : ControllerBase
    {
        private readonly IDebtPaymentService _debtPaymentService;
        private readonly IMapper _mapper;
        public DebtPaymentController(IDebtPaymentService debtPaymentService, IMapper mapper)
        {
            _mapper = mapper;
            _debtPaymentService = debtPaymentService;
        }

        [HttpGet]
        public async Task<IEnumerable<DebtPaymentModel>> GetAll()
        {
            var debtPayments = await _debtPaymentService.GetAll();
            return _mapper.Map<IEnumerable<DebtPaymentModel>>(debtPayments);
        }

        [HttpPut]
        public async Task<Guid> Add(DebtPaymentModel currency)
        {
            var debtPaymentDto = _mapper.Map<DebtPaymentDto>(currency);
            return await _debtPaymentService.Add(debtPaymentDto);
        }

        [HttpPatch]
        public async Task Update(DebtPaymentModel debtPayment)
        {
            var debtPaymentDto = _mapper.Map<DebtPaymentDto>(debtPayment);
            await _debtPaymentService.Update(debtPaymentDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _debtPaymentService.Delete(id);
    }
}
