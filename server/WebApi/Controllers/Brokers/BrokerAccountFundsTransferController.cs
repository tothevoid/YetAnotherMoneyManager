using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.WebApi.Models.Brokers;
using MoneyManager.Application.DTO.Brokers;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [ApiController]
    [Route("api/brokers/fundstransfer")]
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
        public async Task<IEnumerable<BrokerAccountFundsTransferModel>> GetAll(Guid brokerAccountId, bool income)
        {
            var transfers = await _brokerAccountFundsTransferService.GetAllAsync(brokerAccountId, income);
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

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _brokerAccountFundsTransferService.Delete(id);
    }
}
