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

        public async Task<IEnumerable<SecurityTypeDto>> GetAllAsync()
        {
            var securityTypes = await _securityTypeRepo.GetAllAsync();
            return _mapper.Map(securityTypes);
        }

        public async Task UpdateAsync(SecurityTypeDto securityTypeDto)
        {
            var securityType = _mapper.Map(securityTypeDto);
            _securityTypeRepo.Update(securityType);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(SecurityTypeDto securityTypeDto)
        {
            var securityType = _mapper.Map(securityTypeDto);
            securityType.Id = Guid.NewGuid();
            await _securityTypeRepo.AddAsync(securityType);
            await _db.CommitAsync();
            return securityType.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _securityTypeRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }
    }
}
