using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Crypto;
using MoneyManager.WebApi.Models.Securities;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Controllers.Crypto
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CryptocurrencyController: ControllerBase
    {
        private readonly ICryptocurrencyService _cryptocurrencyService;
        private readonly WebApiMapper _mapper;

        public CryptocurrencyController(ICryptocurrencyService cryptocurrencyService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _cryptocurrencyService = cryptocurrencyService;
        }

        [HttpGet]
        public async Task<IEnumerable<CryptocurrencyModel>> GetAll()
        {
            var cryptocurrencies = await _cryptocurrencyService.GetAll();
            return _mapper.Map(cryptocurrencies);
        }

        [HttpPut]
        public async Task<CryptocurrencyModel> Add([FromForm] string cryptocurrencyJson, [FromForm] IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = JsonSerializer.Deserialize<CryptocurrencyModel>(cryptocurrencyJson);
            var cryptocurrencyDto = _mapper.Map(cryptocurrency);
            var result = await _cryptocurrencyService.Add(cryptocurrencyDto, cryptocurrencyIcon);
            return _mapper.Map(result);
        }

        [HttpPatch]
        public async Task<CryptocurrencyModel> Update([FromForm] string cryptocurrencyJson, [FromForm] IFormFile cryptocurrencyIcon)
        {
            var cryptocurrency = JsonSerializer.Deserialize<CryptocurrencyModel>(cryptocurrencyJson);
            var cryptocurrencyDto = _mapper.Map(cryptocurrency);
            var result = await _cryptocurrencyService.Update(cryptocurrencyDto, cryptocurrencyIcon);
            return _mapper.Map(result);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _cryptocurrencyService.Delete(id);


        [HttpGet("icon")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCryptocurrencyIcon(string iconKey)
        {
            Response.Headers.CacheControl = "public, max-age=31536000, immutable";
            var url = await _cryptocurrencyService.GetIconUrl(iconKey);
            return Redirect(url);
        }
    }
}
