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
    public class CryptocurrencyController
    {
        private readonly ICryptocurrencyService _cryptocurrencyService;
        private readonly IMapper _mapper;

        public CryptocurrencyController(ICryptocurrencyService cryptocurrencyService, IMapper mapper)
        {
            _mapper = mapper;
            _cryptocurrencyService = cryptocurrencyService;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptocurrencyModel>> GetAll()
        {
            var cryptocurrencies = await _cryptocurrencyService.GetAll();
            return _mapper.Map<IEnumerable<CryptocurrencyModel>>(cryptocurrencies);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptocurrencyModel cryptocurrency)
        {
            var cryptocurrencyDto = _mapper.Map<CryptocurrencyDto>(cryptocurrency);
            return await _cryptocurrencyService.Add(cryptocurrencyDto);
        }

        [HttpPatch]
        public async Task Update(CryptocurrencyModel cryptocurrency)
        {
            var cryptocurrencyDto = _mapper.Map<CryptocurrencyDto>(cryptocurrency);
            await _cryptocurrencyService.Update(cryptocurrencyDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptocurrencyService.Delete(id);
    }
}
