using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Securities
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

        public async Task<IEnumerable<SecurityDTO>> GetAll()
        {
            var securities = await _securityRepo.GetAll();
            return _mapper.Map<IEnumerable<SecurityDTO>>(securities);
        }

        public async Task Update(SecurityDTO securityTypeDto)
        {
            var security = _mapper.Map<Security>(securityTypeDto);
            await _securityRepo.Update(security);
            _db.Commit();
        }

        public async Task<Guid> Add(SecurityDTO securityDto)
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
