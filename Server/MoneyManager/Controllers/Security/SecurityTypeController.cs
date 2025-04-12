using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;
using MoneyManager.WebApi.Models.Security;

namespace MoneyManager.WebApi.Controllers.Security
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class SecurityTypeController : ControllerBase
    {
        private readonly ISecurityTypeService _securityTypeService;
        private readonly IMapper _mapper;
        public SecurityTypeController(ISecurityTypeService securityTypeService, IMapper mapper)
        {
            _mapper = mapper;
            _securityTypeService = securityTypeService;
        }

        [HttpGet]
        public async Task<IEnumerable<SecurityTypeModel>> GetAll()
        {
            var types = await _securityTypeService.GetAll();
            return _mapper.Map<IEnumerable<SecurityTypeModel>>(types);
        }

        [HttpPut]
        public async Task<Guid> Add(SecurityTypeModel securityType)
        {
            var securityTypeDto = _mapper.Map<SecurityTypeDto>(securityType);
            return await _securityTypeService.Add(securityTypeDto);
        }

        [HttpPatch]
        public async Task Update(SecurityTypeModel securityType)
        {
            var securityTypeDto = _mapper.Map<SecurityTypeDto>(securityType);
            await _securityTypeService.Update(securityTypeDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityTypeService.Delete(id);
    }
}