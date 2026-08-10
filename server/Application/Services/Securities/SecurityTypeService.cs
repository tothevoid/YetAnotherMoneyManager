using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Application.Services.Securities
{
    public class SecurityTypeService: ISecurityTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<SecurityType> _securityTypeRepo;
        private readonly ApplicationMapper _mapper;
        public SecurityTypeService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityTypeRepo = uow.CreateRepository<SecurityType>();
        }

        public async Task<IEnumerable<SecurityTypeDTO>> GetAll()
        {
            var securityTypes = await _securityTypeRepo.GetAll();
            return _mapper.Map(securityTypes);
        }

        public async Task Update(SecurityTypeDTO securityTypeDto)
        {
            var securityType = _mapper.Map(securityTypeDto);
            _securityTypeRepo.Update(securityType);
            await _db.Commit();
        }

        public async Task<Guid> Add(SecurityTypeDTO securityTypeDto)
        {
            var securityType = _mapper.Map(securityTypeDto);
            securityType.Id = Guid.NewGuid();
            await _securityTypeRepo.Add(securityType);
            await _db.Commit();
            return securityType.Id;
        }

        public async Task Delete(Guid id)
        {
            await _securityTypeRepo.Delete(id);
            await _db.Commit();
        }
    }
}
