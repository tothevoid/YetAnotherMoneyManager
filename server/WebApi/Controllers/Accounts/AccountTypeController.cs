using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Accounts
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountTypeService _accountTypeService;
        private readonly WebApiMapper _mapper;
        public AccountTypeController(IAccountTypeService accountTypeService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _accountTypeService = accountTypeService;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountTypeModel>> GetAll()
        {
            var types = await _accountTypeService.GetAll();
            return _mapper.Map(types);
        }

        [HttpPut]
        public async Task<Guid> Add(AccountTypeModel accountType)
        {
            var accountTypeDto = _mapper.Map(accountType);
            return await _accountTypeService.Add(accountTypeDto);
        }

        [HttpPatch]
        public async Task Update(AccountTypeModel accountType)
        {
            var accountTypeDto = _mapper.Map(accountType);
            await _accountTypeService.Update(accountTypeDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _accountTypeService.Delete(id);
    }
}