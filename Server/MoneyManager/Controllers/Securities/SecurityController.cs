using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Models.Securities;

namespace MoneyManager.WebApi.Controllers.Securities
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
        public async Task<Guid> Add(SecurityModel security)
        {
            var securityDto = _mapper.Map<SecurityDto>(security);
            securityDto.CurrencyId = securityDto.Currency.Id;
            securityDto.TypeId = securityDto.Type.Id;
            return await _securityService.Add(securityDto);
        }

        [HttpPatch]
        public async Task Update(SecurityModel security)
        {
            var securityDto = _mapper.Map<SecurityDto>(security);
            await _securityService.Update(securityDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityService.Delete(id);
    }
}