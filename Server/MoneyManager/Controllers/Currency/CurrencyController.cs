using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.WebApi.Models.Currency;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.Interfaces.Currencies;

namespace MoneyManager.WebApi.Controllers.Currency
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;
        public CurrencyController(ICurrencyService currencyService, IMapper mapper)
        {
            _mapper = mapper;
            _currencyService = currencyService;
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
