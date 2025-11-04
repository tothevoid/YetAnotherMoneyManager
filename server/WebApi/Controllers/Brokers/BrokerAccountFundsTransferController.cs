using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
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
    public class BrokerAccountFundsTransferController : ControllerBase
    {
        private readonly IBrokerAccountFundsTransferService _brokerAccountFundsTransferService;
        private readonly IMapper _mapper;

        public BrokerAccountFundsTransferController(IBrokerAccountFundsTransferService brokerAccountFundsTransferService, IMapper mapper)
        {
            _brokerAccountFundsTransferService = brokerAccountFundsTransferService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BrokerAccountFundsTransferModel>> GetAll(Guid brokerAccountId)
        {
            var transfers = await _brokerAccountFundsTransferService.GetAllAsync(brokerAccountId);
            return _mapper.Map<IEnumerable<BrokerAccountFundsTransferModel>>(transfers);
        }

        [HttpPut]
        public async Task<BrokerAccountFundsTransferModel> Add(BrokerAccountFundsTransferModel transferModel)
        {
            var transferDto = _mapper.Map<BrokerAccountFundsTransferDto>(transferModel);
            var result = await _brokerAccountFundsTransferService.Add(transferDto);
            return _mapper.Map<BrokerAccountFundsTransferModel>(result);
        }

        [HttpPatch]
        public async Task Update(BrokerAccountFundsTransferModel transferModel)
        {
            var transferDto = _mapper.Map<BrokerAccountFundsTransferDto>(transferModel);
            await _brokerAccountFundsTransferService.Update(transferDto);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination([FromQuery] Guid brokerAccountId)
        {
            var pagination = await _brokerAccountFundsTransferService
                .GetPagination(brokerAccountId);
            return _mapper.Map<PaginationConfigModel>(pagination);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountFundsTransferService.Delete(id);
    }
}
