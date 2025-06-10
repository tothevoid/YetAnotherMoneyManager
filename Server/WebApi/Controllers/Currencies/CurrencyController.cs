using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.WebApi.Models.Currencies;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Services.User;

namespace MoneyManager.WebApi.Controllers.Currencies
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;
        private readonly IUserProfileService _userProfileService;

        public CurrencyController(ICurrencyService currencyService, IMapper mapper, IUserProfileService userProfileService)
        {
            _mapper = mapper;
            _currencyService = currencyService;
            _userProfileService = userProfileService;
        }

        [HttpGet("SyncRates")]
        public async Task SyncRates()
        {
            var profile = await _userProfileService.Get();
            await _currencyService.SyncRates(profile.Currency);
        }

        [HttpGet]
        public async Task<IEnumerable<CurrencyModel>> GetAll()
        {
            var currencies = await _currencyService.GetAll();
            return _mapper.Map<IEnumerable<CurrencyModel>>(currencies);
        }

        [HttpPut]
        public async Task<Guid> Add(CurrencyModel currency)
        {
            var currencyDto = _mapper.Map<CurrencyDTO>(currency);
            return await _currencyService.Add(currencyDto);
        }

        [HttpPatch]
        public async Task Update(CurrencyModel currency)
        {
            var currencyDto = _mapper.Map<CurrencyDTO>(currency);
            await _currencyService.Update(currencyDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _currencyService.Delete(id);
    }
}
