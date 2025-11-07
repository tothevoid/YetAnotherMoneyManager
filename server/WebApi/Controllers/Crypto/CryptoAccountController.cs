using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.WebApi.Models.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.WebApi.Models.Securities;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Crypto
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CryptoAccountController: ControllerBase
    {
        private readonly ICryptoAccountService _cryptoAccountService;
        private readonly IMapper _mapper;
        public CryptoAccountController(ICryptoAccountService cryptoAccountService, IMapper mapper)
        {
            _mapper = mapper;
            _cryptoAccountService = cryptoAccountService;
        }

        [HttpGet(nameof(GetById))]
        public async Task<CryptoAccountModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _cryptoAccountService.GetById(id);
            return _mapper.Map<CryptoAccountModel>(brokerAccount);
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoAccountModel>> GetAll()
        {
            var cryptoAccounts = await _cryptoAccountService.GetAll();
            return _mapper.Map<IEnumerable<CryptoAccountModel>>(cryptoAccounts);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoAccountModel cryptAccount)
        {
            var cryptoAccountDto = _mapper.Map<CryptoAccountDto>(cryptAccount);
            return await _cryptoAccountService.Add(cryptoAccountDto);
        }

        [HttpPatch]
        public async Task Update(CryptoAccountModel cryptAccount)
        {
            var cryptAccountDto = _mapper.Map<CryptoAccountDto>(cryptAccount);
            await _cryptoAccountService.Update(cryptAccountDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoAccountService.Delete(id);
    }
}
