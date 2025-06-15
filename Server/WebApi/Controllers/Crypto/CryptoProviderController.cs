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
    public class CryptoProviderController
    {
        private readonly ICryptoProviderService _cryptoProviderService;
        private readonly IMapper _mapper;
        public CryptoProviderController(ICryptoProviderService cryptoProviderService, IMapper mapper)
        {
            _mapper = mapper;
            _cryptoProviderService = cryptoProviderService;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoProviderModel>> GetAll()
        {
            var cryptoProviders = await _cryptoProviderService.GetAll();
            return _mapper.Map<IEnumerable<CryptoProviderModel>>(cryptoProviders);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoProviderModel cryptoProvider)
        {
            var cryptoProviderDto = _mapper.Map<CryptoProviderDto>(cryptoProvider);
            return await _cryptoProviderService.Add(cryptoProviderDto);
        }

        [HttpPatch]
        public async Task Update(CryptoProviderModel cryptoProvider)
        {
            var cryptoProviderDto = _mapper.Map<CryptoProviderDto>(cryptoProvider);
            await _cryptoProviderService.Update(cryptoProviderDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoProviderService.Delete(id);
    }
}
