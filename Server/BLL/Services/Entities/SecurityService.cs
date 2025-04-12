using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;

namespace MoneyManager.BLL.Services.Entities
{
    public class SecurityService: ISecurityService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<Security> _securityRepo;
        private readonly IMapper _mapper;
        public SecurityService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityRepo = uow.CreateRepository<Security>();
        }

        public IEnumerable<SecurityDto> GetAll(bool onlyActive)
        {
            var securities = _securityRepo.GetAll();
            return _mapper.Map<IEnumerable<SecurityDto>>(securities);
        }

        public async Task Update(SecurityDto securityTypeDto)
        {
            var security = _mapper.Map<Security>(securityTypeDto);
            await _securityRepo.Update(security);
            _db.Commit();
        }

        public async Task<Guid> Add(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);
            security.Id = Guid.NewGuid();
            await _securityRepo.Add(security);
            _db.Commit();
            return security.Id;
        }

        public async Task Delete(Guid id)
        {
            await _securityRepo.Delete(id);
            _db.Commit();
        }
    }
}
