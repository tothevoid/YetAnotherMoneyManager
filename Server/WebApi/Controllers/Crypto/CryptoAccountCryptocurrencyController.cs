using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.WebApi.Models.Crypto;

namespace MoneyManager.WebApi.Controllers.Crypto
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class CryptoAccountCryptocurrencyController
    {
        private readonly ICryptoAccountCryptocurrencyService _cryptoAccountCryptocurrencyService;
        private readonly IMapper _mapper;

        public CryptoAccountCryptocurrencyController(ICryptoAccountCryptocurrencyService cryptoAccountCryptocurrencyService, IMapper mapper)
        {
            _mapper = mapper;
            _cryptoAccountCryptocurrencyService = cryptoAccountCryptocurrencyService;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoAccountCryptocurrencyModel>> GetAll()
        {
            var cryptoAccountCryptocurrencies = await _cryptoAccountCryptocurrencyService.GetAll();
            return _mapper.Map<IEnumerable<CryptoAccountCryptocurrencyModel>>(cryptoAccountCryptocurrencies);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoAccountCryptocurrencyModel cryptoAccountCryptocurrency)
        {
            var cryptoAccountCryptocurrencyDto = _mapper.Map<CryptoAccountCryptocurrencyDto>(cryptoAccountCryptocurrency);
            return await _cryptoAccountCryptocurrencyService.Add(cryptoAccountCryptocurrencyDto);
        }

        [HttpPatch]
        public async Task Update(CryptoAccountCryptocurrencyModel brokerAccount)
        {
            var cryptoAccountCryptocurrencyDto = _mapper.Map<CryptoAccountCryptocurrencyDto>(brokerAccount);
            await _cryptoAccountCryptocurrencyService.Update(cryptoAccountCryptocurrencyDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoAccountCryptocurrencyService.Delete(id);
    }
}
