using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.WEB.Model;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;

namespace MoneyManager.WEB.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        public SecurityController(ISecurityService securityService, IMapper mapper)
        {
            _mapper = mapper;
            _securityService = securityService;
        }

        [HttpPost("GetAll")]
        public IEnumerable<SecurityModel> GetAll()
        {
            var securities = _securityService.GetAll(true);
            return _mapper.Map<IEnumerable<SecurityModel>>(securities);
        }

        [HttpPut]
        public async Task<Guid> Add(SecurityModel account)
        {
            var securityDto = _mapper.Map<SecurityDto>(account);
            securityDto.CurrencyId = securityDto.Currency.Id;
            securityDto.TypeId = securityDto.Type.Id;
            return await _securityService.Add(securityDto);
        }

        [HttpPatch]
        public async Task Update(SecurityModel account)
        {
            var securityDto = _mapper.Map<SecurityDto>(account);
            await _securityService.Update(securityDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityService.Delete(id);
    }
}