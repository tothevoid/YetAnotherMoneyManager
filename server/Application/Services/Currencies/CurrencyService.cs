using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.Integrations.Currency;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Infrastructure.Entities.User;

namespace MoneyManager.Application.Services.Currencies
{
    public class CurrencyService: ICurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Currency> _currencyRepo;
        private readonly ApplicationMapper _mapper;
        private readonly ICurrencyGrabber _currencyGrabber;
      
        public CurrencyService(IUnitOfWork uow, ApplicationMapper mapper, ICurrencyGrabber currencyGrabber)
        {
            _db = uow;
            _mapper = mapper;
            _currencyRepo = uow.CreateRepository<Currency>();
            _currencyGrabber = currencyGrabber;
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllAsync()
        {
            var transactions = await _currencyRepo.GetAllAsync();
            return _mapper.Map(transactions);
        }

        public async Task<CurrencyDto> GetByIdAsync(Guid id)
        {
            var transactions = await _currencyRepo.GetByIdAsync(id);
            return _mapper.Map(transactions);
        }

        //TODO: Guid parameter instead of DTO
        //TODO: Should be in separate service
        public async Task SyncRatesAsync(CurrencyDto mainCurrency)
        {
            var currencies = (await _currencyRepo.GetAllAsync(disableTracking: false)).ToList();
            var currenciesNames = currencies.Select(x => x.Name).ToHashSet();

            var rates = await _currencyGrabber.GetRatesAsync(mainCurrency.Name, currenciesNames);

            foreach (var currency in currencies)
            {
                if (currency.Id == mainCurrency.Id)
                {
                    currency.Rate = 1;
                    continue;
                }

                currency.Rate = rates.TryGetValue(currency.Name, out var rate) ? rate : 1;
            }
            await _db.CommitAsync();
        }

        public async Task UpdateAsync(CurrencyDto currencyDto)
        {
            var currency = _mapper.Map(currencyDto);
            _currencyRepo.Update(currency);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(CurrencyDto currencyDto)
        {
            var currency = _mapper.Map(currencyDto);
            currency.Id = Guid.NewGuid();
            await _currencyRepo.AddAsync(currency);
            await _db.CommitAsync();
            return currency.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _currencyRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }
    }
}
