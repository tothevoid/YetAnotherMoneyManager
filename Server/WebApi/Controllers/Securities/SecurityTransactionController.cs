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
    public class SecurityTransactionController : ControllerBase
    {
        private readonly ISecurityTransactionService _securityTransactionService;
        private readonly IMapper _mapper;
        public SecurityTransactionController(ISecurityTransactionService securityTransactionService, IMapper mapper)
        {
            _mapper = mapper;
            _securityTransactionService = securityTransactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<SecurityTransactionModel>> GetAll()
        {
            var securityTransactions = await _securityTransactionService.GetAll();
            return _mapper.Map<IEnumerable<SecurityTransactionModel>>(securityTransactions);
        }

        [HttpPut]
        public async Task<Guid> Add(SecurityTransactionModel securityTransaction)
        {
            var securityTransactionDto = _mapper.Map<SecurityTransactionDTO>(securityTransaction);
            securityTransactionDto.SecurityId = securityTransactionDto.Security.Id;
            return await _securityTransactionService.Add(securityTransactionDto);
        }

        [HttpPatch]
        public async Task Update(SecurityTransactionModel securityTransaction)
        {
            var securityTransactionDto = _mapper.Map<SecurityTransactionDTO>(securityTransaction);
            await _securityTransactionService.Update(securityTransactionDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _securityTransactionService.Delete(id);
    }
}