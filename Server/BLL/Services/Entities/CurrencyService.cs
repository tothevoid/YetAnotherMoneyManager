using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public class CurrencyService: ICurrencyService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Currency> _currencyRepo;
        private readonly IMapper _mapper;
        public CurrencyService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _currencyRepo = uow.CreateRepository<Currency>();
        }

        public async Task<IEnumerable<CurrencyDTO>> GetAll()
        {
            var transactions = await _currencyRepo.GetAll();
            return _mapper.Map<IEnumerable<CurrencyDTO>>(transactions);
        }

        public async Task Update(CurrencyDTO currencyDto)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            await _currencyRepo.Update(currency);
            _db.Commit();
        }

        public async Task<Guid> Add(CurrencyDTO currencyDto)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            currency.Id = Guid.NewGuid();
            await _currencyRepo.Add(currency);
            _db.Commit();
            return currency.Id;
        }

        public async Task Delete(Guid id)
        {
            await _currencyRepo.Delete(id);
            _db.Commit();
        }
    }
}
