using Audex.Application.Interfaces.Crypto;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Crypto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Audex.WebApi.Controllers.Crypto
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CryptoAccountStatsController : ControllerBase
    {
        private readonly ICryptoAccountStatsService _cryptoAccountStatsService;
        private readonly WebApiMapper _mapper;

        public CryptoAccountStatsController(
            ICryptoAccountStatsService cryptoAccountStatsService,
            WebApiMapper mapper)
        {
            _cryptoAccountStatsService = cryptoAccountStatsService;
            _mapper = mapper;
        }

        [HttpGet(nameof(GetStats))]
        public async Task<CryptoAccountStatsModel> GetStats()
        {
            var stats = await _cryptoAccountStatsService.GetStatsAsync();
            return _mapper.Map(stats);
        }

        [HttpGet(nameof(GetStatsByCryptoAccount))]
        public async Task<CryptoAccountStatsModel> GetStatsByCryptoAccount([FromQuery] Guid cryptoAccountId)
        {
            var stats = await _cryptoAccountStatsService.GetStatsByCryptoAccountAsync(cryptoAccountId);
            return _mapper.Map(stats);
        }
    }
}
