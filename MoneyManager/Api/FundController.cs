using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.WEB.Model;
using AutoMapper;
using MoneyManager.BLL.DTO;

namespace MoneyManager.WEB.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly IFundService _fundService;
        private readonly IMapper _mapper;
        public FundController(IFundService fundService, IMapper mapper)
        {
            _mapper = mapper;
            _fundService = fundService;
        }

        [HttpGet]
        public async Task<IEnumerable<FundModel>> GetAll()
        {
            var funds = await _fundService.GetAll();
            return _mapper.Map<IEnumerable<FundModel>>(funds);
        }

        [HttpPut]
        public async Task<Guid> Add(FundModel fund)
        {
            var fundDTO = _mapper.Map<FundDTO>(fund);
            return await _fundService.Add(fundDTO);
        }

        [HttpPatch]
        public async Task Update(FundModel fund)
        {
            var fundDTO = _mapper.Map<FundDTO>(fund);
            await _fundService.Update(fundDTO);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _fundService.Delete(id);
        }
    }
}