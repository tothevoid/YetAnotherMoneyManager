using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
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
    public class BrokerAccountFundsTransferController : ControllerBase
    {
        private readonly IBrokerAccountFundsTransferService _brokerAccountFundsTransferService;
        private readonly WebApiMapper _mapper;

        public BrokerAccountFundsTransferController(IBrokerAccountFundsTransferService brokerAccountFundsTransferService, WebApiMapper mapper)
        {
            _brokerAccountFundsTransferService = brokerAccountFundsTransferService;
            _mapper = mapper;
        }

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<BrokerAccountFundsTransferModel>> GetAll(GetAllBrokerAccountFundTransferQuery query)
        {
            var transfers = await _brokerAccountFundsTransferService.GetAllAsync(query.BrokerAccountId, 
                query.PageIndex, query.RecordsQuantity);
            return _mapper.Map(transfers);
        }

        [HttpPut]
        public async Task<BrokerAccountFundsTransferModel> Add(BrokerAccountFundsTransferModel transferModel)
        {
            var transferDto = _mapper.Map(transferModel);
            var result = await _brokerAccountFundsTransferService.AddAsync(transferDto);
            return _mapper.Map(result);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountFundsTransferModel transferModel)
        {
            var transferDto = _mapper.Map(transferModel);
            await _brokerAccountFundsTransferService.UpdateAsync(transferDto);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination()
        {
            var pagination = await _brokerAccountFundsTransferService
                .GetPaginationAsync();
            return _mapper.Map(pagination);
        }

        [HttpGet(nameof(GetPaginationByBrokerAccount))]
        public async Task<PaginationConfigModel> GetPaginationByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var pagination = await _brokerAccountFundsTransferService
                .GetPaginationByBrokerAccountAsync(brokerAccountId);
            return _mapper.Map(pagination);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountFundsTransferService.DeleteAsync(id);
    }
}
