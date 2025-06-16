using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.WebApi.Models.Crypto;
using Microsoft.AspNetCore.Http;
using MoneyManager.WebApi.Models.Securities;

namespace MoneyManager.WebApi.Controllers.Crypto
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class CryptocurrencyController: ControllerBase
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
        public async Task<Guid> Add([FromForm] string cryptocurrencyJson, [FromForm] IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = JsonSerializer.Deserialize<CryptocurrencyModel>(cryptocurrencyJson);
            var cryptocurrencyDto = _mapper.Map<CryptocurrencyDto>(cryptocurrency);
            return await _cryptocurrencyService.Add(cryptocurrencyDto, cryptocurrencyIcon);
        }

        [HttpPatch]
        public async Task Update([FromForm] string cryptocurrencyJson, [FromForm] IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = JsonSerializer.Deserialize<CryptocurrencyModel>(cryptocurrencyJson);
            var cryptocurrencyDto = _mapper.Map<CryptocurrencyDto>(cryptocurrency);
            await _cryptocurrencyService.Update(cryptocurrencyDto, cryptocurrencyIcon);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptocurrencyService.Delete(id);


        [HttpGet("icon")]
        public async Task<IActionResult> GetCryptocurrencyIcon(string iconKey)
        {
            var url = await _cryptocurrencyService.GetIconUrl(iconKey);
            return Redirect(url);
        }
    }
}
