using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Audex.Application.DTO.Crypto;
using Audex.Application.Interfaces.Crypto;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Crypto;
using Microsoft.AspNetCore.Authorization;

namespace Audex.WebApi.Controllers.Crypto
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CryptoAccountCryptocurrencyController: ControllerBase
    {
        private readonly ICryptoAccountCryptocurrencyService _cryptoAccountCryptocurrencyService;
        private readonly WebApiMapper _mapper;

        public CryptoAccountCryptocurrencyController(ICryptoAccountCryptocurrencyService cryptoAccountCryptocurrencyService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _cryptoAccountCryptocurrencyService = cryptoAccountCryptocurrencyService;
        }

        [HttpGet(nameof(GetByCryptoAccount))]
        public async Task<IEnumerable<CryptoAccountCryptocurrencyModel>> GetByCryptoAccount(Guid cryptoAccountId)
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyService.GetByCryptoAccountAsync(cryptoAccountId);
            return _mapper.Map(cryptoAccountCryptocurrencies);
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoAccountCryptocurrencyModel>> GetAll()
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyService.GetAllAsync();
            return _mapper.Map(cryptoAccountCryptocurrencies);
        }

        [HttpGet(nameof(GetTotalBalanceByCryptoAccount))]
        public async Task<decimal> GetTotalBalanceByCryptoAccount([FromQuery] Guid cryptoAccountId)
        {
            return await _cryptoAccountCryptocurrencyService.GetTotalBalanceByCryptoAccountAsync(cryptoAccountId);
        }

        [HttpGet(nameof(GetTotalBalance))]
        public async Task<decimal> GetTotalBalance()
        {
            return await _cryptoAccountCryptocurrencyService.GetTotalBalanceAsync();
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoAccountCryptocurrencyModel cryptoAccountCryptocurrency)
        {
            var cryptoAccountCryptocurrencyDto = _mapper.Map(cryptoAccountCryptocurrency);
            return await _cryptoAccountCryptocurrencyService.AddAsync(cryptoAccountCryptocurrencyDto);
        }

        [HttpPatch]
        public async Task Update(CryptoAccountCryptocurrencyModel brokerAccount)
        {
            var cryptoAccountCryptocurrencyDto = _mapper.Map(brokerAccount);
            await _cryptoAccountCryptocurrencyService.UpdateAsync(cryptoAccountCryptocurrencyDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoAccountCryptocurrencyService.DeleteAsync(id);
    }
}
