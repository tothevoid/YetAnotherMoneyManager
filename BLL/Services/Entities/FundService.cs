using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using MoneyManager.Common;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public class FundService : IFundService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Fund> _fundRepo;
        private readonly IMapper _mapper;
        public FundService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _fundRepo = uow.CreateRepository<Fund>();
        }

        public async Task<IEnumerable<FundDTO>> GetAll()
        {
            var transactions = await _fundRepo.GetAll();
            return _mapper.Map<IEnumerable<FundDTO>>(transactions);
        }

        public async Task Update(FundDTO fundDTO)
        {
            var fund = _mapper.Map<Fund>(fundDTO);
            await _fundRepo.Update(fund);
            _db.Commit();
        }

        public async Task<Guid> Add(FundDTO transactionDTO)
        {
            var fund = _mapper.Map<Fund>(transactionDTO);
            fund.Id = Guid.NewGuid();
            await _fundRepo.Add(fund);
            _db.Commit();
            return fund.Id;
        }

        public async Task Delete(Guid id)
        {
            await _fundRepo.Delete(id);
            _db.Commit();
        }
    }
}