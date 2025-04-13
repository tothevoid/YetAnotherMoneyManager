using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.WebApi.Models.Security;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;

namespace MoneyManager.WebApi.Controllers.Security
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class SecurityTransactionController : ControllerBase
    {
        private readonly ISecurityTransactionService _securityTransactionService;
        private readonly IMapper _mapper;
        public SecurityTransactionController(ISecurityTransactionService securityTransactionService, IMapper mapper)
        {
            _mapper = mapper;
            _securityTransactionService = securityTransactionService;
        }

        [HttpPost("GetAll")]
        public IEnumerable<SecurityTransactionModel> GetAll()
        {
            var securities = _securityTransactionService.GetAll();
            return _mapper.Map<IEnumerable<SecurityTransactionModel>>(securities);
        }

        [HttpPut]
        public async Task<Guid> Add(SecurityTransactionModel securityTransaction)
        {
            var securityTransactionDto = _mapper.Map<SecurityTransactionDto>(securityTransaction);
            securityTransactionDto.SecurityId = securityTransactionDto.Security.Id;
            return await _securityTransactionService.Add(securityTransactionDto);
        }

        [HttpPatch]
        public async Task Update(SecurityTransactionModel securityTransaction)
        {
            var securityTransactionDto = _mapper.Map<SecurityTransactionDto>(securityTransaction);
            await _securityTransactionService.Update(securityTransactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityTransactionService.Delete(id);
    }
}