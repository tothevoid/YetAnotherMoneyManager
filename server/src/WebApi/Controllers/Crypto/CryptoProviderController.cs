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
    public class CryptoProviderController: ControllerBase
    {
        private readonly ICryptoProviderService _cryptoProviderService;
        private readonly WebApiMapper _mapper;
        public CryptoProviderController(ICryptoProviderService cryptoProviderService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _cryptoProviderService = cryptoProviderService;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptoProviderModel>> GetAll()
        {
            var cryptoProviders = await _cryptoProviderService.GetAllAsync();
            return _mapper.Map(cryptoProviders);
        }

        [HttpPut]
        public async Task<Guid> Add(CryptoProviderModel cryptoProvider)
        {
            var cryptoProviderDto = _mapper.Map(cryptoProvider);
            return await _cryptoProviderService.AddAsync(cryptoProviderDto);
        }

        [HttpPatch]
        public async Task Update(CryptoProviderModel cryptoProvider)
        {
            var cryptoProviderDto = _mapper.Map(cryptoProvider);
            await _cryptoProviderService.UpdateAsync(cryptoProviderDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptoProviderService.DeleteAsync(id);
    }
}
