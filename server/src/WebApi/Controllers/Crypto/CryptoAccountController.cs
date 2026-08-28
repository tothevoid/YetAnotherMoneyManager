using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Crypto;
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
        private readonly WebApiMapper _mapper;
        public CryptoAccountController(ICryptoAccountService cryptoAccountService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _cryptoAccountService = cryptoAccountService;
        }

        [HttpGet(nameof(GetById))]
        public async Task<CryptoAccountModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _cryptoAccountService.GetByIdAsync(id);
            return _mapper.Map(brokerAccount);
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoAccountModel>> GetAll()
        {
            var cryptoAccounts = await _cryptoAccountService.GetAllAsync();
            return _mapper.Map(cryptoAccounts);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoAccountModel cryptAccount)
        {
            var cryptoAccountDto = _mapper.Map(cryptAccount);
            return await _cryptoAccountService.AddAsync(cryptoAccountDto);
        }

        [HttpPatch]
        public async Task Update(CryptoAccountModel cryptAccount)
        {
            var cryptAccountDto = _mapper.Map(cryptAccount);
            await _cryptoAccountService.UpdateAsync(cryptAccountDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoAccountService.DeleteAsync(id);
    }
}
