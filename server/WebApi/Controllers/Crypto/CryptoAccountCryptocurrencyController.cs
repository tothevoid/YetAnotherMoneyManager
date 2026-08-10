using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Crypto;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Crypto
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
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyService.GetByCryptoAccount(cryptoAccountId);
            return _mapper.Map(cryptoAccountCryptocurrencies);
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoAccountCryptocurrencyModel>> GetAll()
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyService.GetAll();
            return _mapper.Map(cryptoAccountCryptocurrencies);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoAccountCryptocurrencyModel cryptoAccountCryptocurrency)
        {
            var cryptoAccountCryptocurrencyDto = _mapper.Map(cryptoAccountCryptocurrency);
            return await _cryptoAccountCryptocurrencyService.Add(cryptoAccountCryptocurrencyDto);
        }

        [HttpPatch]
        public async Task Update(CryptoAccountCryptocurrencyModel brokerAccount)
        {
            var cryptoAccountCryptocurrencyDto = _mapper.Map(brokerAccount);
            await _cryptoAccountCryptocurrencyService.Update(cryptoAccountCryptocurrencyDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoAccountCryptocurrencyService.Delete(id);
    }
}
