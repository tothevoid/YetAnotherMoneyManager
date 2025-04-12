using AutoMapper;
using MoneyManager.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.BLL.Interfaces.Entities;
using MoneyManager.BLL.DTO;
using System;

namespace BLL.Services.Entities
{
    public class SecurityTypeService: ISecurityTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<SecurityType> _securityTypeRepo;
        private readonly IMapper _mapper;
        public SecurityTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityTypeRepo = uow.CreateRepository<SecurityType>();
        }

        public async Task<IEnumerable<SecurityTypeDto>> GetAll()
        {
            var securityTypes = await _securityTypeRepo.GetAll();
            return _mapper.Map<IEnumerable<SecurityTypeDto>>(securityTypes);
        }

        public async Task Update(SecurityTypeDto securityTypeDto)
        {
            var securityType = _mapper.Map<SecurityType>(securityTypeDto);
            await _securityTypeRepo.Update(securityType);
            _db.Commit();
        }

        public async Task<Guid> Add(SecurityTypeDto securityTypeDto)
        {
            var securityType = _mapper.Map<SecurityType>(securityTypeDto);
            securityType.Id = Guid.NewGuid();
            await _securityTypeRepo.Add(securityType);
            _db.Commit();
            return securityType.Id;
        }

        public async Task Delete(Guid id)
        {
            await _securityTypeRepo.Delete(id);
            _db.Commit();
        }
    }
}
