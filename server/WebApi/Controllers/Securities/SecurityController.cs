using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Text.Json;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Models.Securities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Securities
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet(nameof(GetById))]
        public async Task<SecurityModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _securityService.GetById(id);
            return _mapper.Map<SecurityModel>(brokerAccount);
        }

        [HttpGet(nameof(GetStats))]
        public async Task<SecurityStatsModel> GetStats([FromQuery] Guid securityId)
        {
            var stats = await _securityService.GetStats(securityId);
            return _mapper.Map<SecurityStatsModel>(stats);
        }

        [HttpGet("icon")]
        public async Task<IActionResult> GetSecurityIcon(string iconKey)
        {
            var url = await _securityService.GetIconUrl(iconKey);
            return Redirect(url);
        }

        [HttpGet(nameof(GetTickerHistory))]
        public async Task<IEnumerable<SecurityHistoryValueModel>> GetTickerHistory([FromQuery] string ticker)
        {
            var brokerAccount = await _securityService.GetTickerHistory(ticker);
            return _mapper.Map<IEnumerable<SecurityHistoryValueModel>>(brokerAccount);
        }

        [HttpPut]
        public async Task<SecurityDTO> Add([FromForm] string securityJson, [FromForm] IFormFile securityIcon)
        {
            var security = JsonSerializer.Deserialize<SecurityModel>(securityJson);
            var securityDto = _mapper.Map<SecurityDTO>(security);
            return await _securityService.Add(securityDto, securityIcon);
        }

        [HttpPatch]
        public async Task<SecurityDTO> Update([FromForm] string securityJson, [FromForm] IFormFile securityIcon)
        {
            var security = JsonSerializer.Deserialize<SecurityModel>(securityJson);
            var securityDto = _mapper.Map<SecurityDTO>(security);
            return await _securityService.Update(securityDto, securityIcon);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityService.Delete(id);
    }
}