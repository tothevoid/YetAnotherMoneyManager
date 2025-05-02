using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.WebApi.Models.Brokers;

namespace MoneyManager.WebApi.Controllers.Securities
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class SecurityTransactionController : ControllerBase
    {
        private readonly ISecurityTransactionService _securityTransactionService;
        private readonly IMapper _mapper;
        public SecurityTransactionController(ISecurityTransactionService securityTransactionService, IMapper mapper)
        {
            _mapper = mapper;
            _securityTransactionService = securityTransactionService;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<SecurityTransactionModel>> GetAll(SecurityTransactionsRequestModel request)
        {
            var securityTransactions = await _securityTransactionService
                .GetAll(request.BrokerAccountId, request.RecordsQuantity, request.PageIndex);
            return _mapper.Map<IEnumerable<SecurityTransactionModel>>(securityTransactions);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<SecurityTransactionPaginationModel> GetPagination([FromQuery] Guid brokerAccountId)
        {
            var pagination = await _securityTransactionService
                .GetPagination(brokerAccountId);
            return _mapper.Map<SecurityTransactionPaginationModel>(pagination);
        }

        [HttpPut]
        public async Task<Guid> Add(SecurityTransactionModel securityTransaction)
        {
            var securityTransactionDto = _mapper.Map<SecurityTransactionDTO>(securityTransaction);
            return await _securityTransactionService.Add(securityTransactionDto);
        }

        [HttpPatch]
        public async Task Update(SecurityTransactionModel securityTransaction)
        {
            var securityTransactionDto = _mapper.Map<SecurityTransactionDTO>(securityTransaction);
            await _securityTransactionService.Update(securityTransactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityTransactionService.Delete(id);
    }
}