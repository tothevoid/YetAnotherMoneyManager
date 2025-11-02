using AutoMapper;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Infrastructure.Entities.Banks;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Banks
{
    public class BankService : IBankService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IMapper _mapper;

        public BankService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _bankRepo = uow.CreateRepository<Bank>();
        }

        public async Task<IEnumerable<BankDto>> GetAll()
        {
            var banks = await _bankRepo.GetAll();
            return _mapper.Map<IEnumerable<BankDto>>(banks);
        }

        public async Task<BankDto> GetById(Guid id)
        {
            var bank = await _bankRepo.GetById(id);
            return _mapper.Map<BankDto>(bank);
        }

        public async Task<BankDto> Add(BankDto bankDto)
        {
            var bank = _mapper.Map<Bank>(bankDto);
            await _bankRepo.Add(bank);
            await _db.Commit();
            return _mapper.Map<BankDto>(bank);
        }

        public async Task<BankDto> Update(BankDto bankDto)
        {
            var bank = await _bankRepo.GetById(bankDto.Id);
            if (bank == null) return null;
            _mapper.Map(bankDto, bank);
            await _db.Commit();
            return _mapper.Map<BankDto>(bank);
        }

        public async Task<bool> Delete(Guid id)
        {
            await _bankRepo.Delete(id);
            await _db.Commit();
            return true;
        }
    }
}