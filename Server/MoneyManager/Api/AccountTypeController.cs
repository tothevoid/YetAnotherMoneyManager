﻿using System.Threading.Tasks;
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
    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountTypeService _accountTypeService;
        private readonly IMapper _mapper;
        public AccountTypeController(IAccountTypeService accountTypeService, IMapper mapper)
        {
            _mapper = mapper;
            _accountTypeService = accountTypeService;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountTypeModel>> GetAll()
        {
            var types = await _accountTypeService.GetAll();
            return _mapper.Map<IEnumerable<AccountTypeModel>>(types);
        }

        [HttpPut]
        public async Task<Guid> Add(AccountTypeModel accountType)
        {
            var accountTypeDto = _mapper.Map<AccountTypeDTO>(accountType);
            return await _accountTypeService.Add(accountTypeDto);
        }

        [HttpPatch]
        public async Task Update(AccountTypeModel accountType)
        {
            var accountTypeDto = _mapper.Map<AccountTypeDTO>(accountType);
            await _accountTypeService.Update(accountTypeDto);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _accountTypeService.Delete(id);
    }
}