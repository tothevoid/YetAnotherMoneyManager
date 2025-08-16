using AutoMapper;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.Interfaces.Currencies;
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
        private readonly IMapper _mapper;
        private readonly ICurrencyGrabber _currencyGrabber;
      
        public CurrencyService(IUnitOfWork uow, IMapper mapper, ICurrencyGrabber currencyGrabber)
        {
            _db = uow;
            _mapper = mapper;
            _currencyRepo = uow.CreateRepository<Currency>();
            _currencyGrabber = currencyGrabber;
        }

        public async Task<IEnumerable<CurrencyDTO>> GetAll()
        {
            var transactions = await _currencyRepo.GetAll();
            return _mapper.Map<IEnumerable<CurrencyDTO>>(transactions);
        }

        public async Task SyncRates(CurrencyDTO mainCurrency)
        {
            var currencies = (await _currencyRepo.GetAll(disableTracking: false)).ToList();
            var currenciesNames = currencies.Select(x => x.Name).ToHashSet();

            var rates = await _currencyGrabber.GetRates(mainCurrency.Name, currenciesNames);

            foreach (var currency in currencies)
            {
                if (currency.Id == mainCurrency.Id)
                {
                    currency.Rate = 1;
                    continue;
                }

                currency.Rate = rates.TryGetValue(currency.Name, out var rate) ? rate : 1;
            }
            await _db.Commit();
        }

        public async Task Update(CurrencyDTO currencyDto)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            _currencyRepo.Update(currency);
            await _db.Commit();
        }

        public async Task<Guid> Add(CurrencyDTO currencyDto)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            currency.Id = Guid.NewGuid();
            await _currencyRepo.Add(currency);
            await _db.Commit();
            return currency.Id;
        }

        public async Task Delete(Guid id)
        {
            await _currencyRepo.Delete(id);
            await _db.Commit();
        }
    }
}
