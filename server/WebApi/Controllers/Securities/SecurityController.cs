using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Text.Json;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.WebApi.Mappings;
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

        private readonly WebApiMapper _mapper;

        public SecurityController(ISecurityService securityService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _securityService = securityService;
        }

        [HttpGet]
        public async Task<IEnumerable<SecurityModel>> GetAll()
        {
            var securities = await _securityService.GetAllAsync();
            return _mapper.Map(securities);
        }

        [HttpGet(nameof(GetById))]
        public async Task<SecurityModel> GetById([FromQuery] Guid id)
        {
            var brokerAccount = await _securityService.GetByIdAsync(id);
            return _mapper.Map(brokerAccount);
        }

        [HttpGet(nameof(GetStats))]
        public async Task<SecurityStatsModel> GetStats([FromQuery] Guid securityId)
        {
            var stats = await _securityService.GetStatsAsync(securityId);
            return _mapper.Map(stats);
        }

        [HttpGet("icon")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSecurityIcon(string iconKey)
        {
            var file = await _securityService.GetIconStreamAsync(iconKey);
            if (file == null)
            {
                return NotFound();
            }

            Response.Headers.CacheControl = "public, max-age=31536000, immutable";
            return File(file.Stream, file.ContentType);
        }

        [HttpGet(nameof(GetTickerHistory))]
        public async Task<IEnumerable<SecurityHistoryValueModel>> GetTickerHistory([FromQuery] string ticker)
        {
            var brokerAccount = await _securityService.GetTickerHistoryAsync(ticker);
            return _mapper.Map(brokerAccount);
        }

        [HttpPut]
        public async Task<SecurityDto> Add([FromForm] string securityJson, [FromForm] IFormFile securityIcon)
        {
            var security = JsonSerializer.Deserialize<SecurityModel>(securityJson);
            var securityDto = _mapper.Map(security);
            return await _securityService.AddAsync(securityDto, securityIcon);
        }

        [HttpPatch]
        public async Task<SecurityDto> Update([FromForm] string securityJson, [FromForm] IFormFile securityIcon)
        {
            var security = JsonSerializer.Deserialize<SecurityModel>(securityJson);
            var securityDto = _mapper.Map(security);
            return await _securityService.UpdateAsync(securityDto, securityIcon);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityService.DeleteAsync(id);
    }
}