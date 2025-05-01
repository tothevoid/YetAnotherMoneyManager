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

        [HttpGet]
        public async Task<IEnumerable<SecurityModel>> GetAll()
        {
            var securities = await _securityService.GetAll();
            return _mapper.Map<IEnumerable<SecurityModel>>(securities);
        }

        [HttpPut]
        public async Task<Guid> Add(SecurityModel security)
        {
            var securityDto = _mapper.Map<SecurityDTO>(security);
            return await _securityService.Add(securityDto);
        }

        [HttpPatch]
        public async Task Update(SecurityModel security)
        {
            var securityDto = _mapper.Map<SecurityDTO>(security);
            await _securityService.Update(securityDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityService.Delete(id);
    }
}