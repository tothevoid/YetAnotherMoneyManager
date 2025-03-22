using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.WEB.Model;
using AutoMapper;
using MoneyManager.BLL.DTO;
using BLL.DTO;

namespace MoneyManager.WEB.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountModel>> GetAll()
        {
            var accounts = await _accountService.GetAll();
            return _mapper.Map<IEnumerable<AccountModel>>(accounts);
        }

        [HttpPut]
        public async Task<Guid> Add(AccountModel account)
        {
            var accountDTO = _mapper.Map<AccountDTO>(account);
            accountDTO.CurrencyId = accountDTO.Currency.Id; 
            return await _accountService.Add(accountDTO);
        }

        [HttpPatch]
        public async Task Update(AccountModel account)
        {
            var accountDTO = _mapper.Map<AccountDTO>(account);
            accountDTO.CurrencyId = accountDTO.Currency.Id;
            await _accountService.Update(accountDTO);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _accountService.Delete(id);

        [HttpPost(nameof(Transfer))]
        public async Task Transfer(AccountTransferModel accountTransfer)
        {
            var transferDto = _mapper.Map<AccountTransferDto>(accountTransfer);
            await _accountService.Transfer(transferDto);
        }

        [HttpGet(nameof(GetSummary))]
        public async Task<AccountCurrencySummaryModel[]> GetSummary()
        {
            var result =  await _accountService.GetSummary();
            var summaryModel = _mapper.Map<AccountCurrencySummaryModel[]>(result);
            return summaryModel;
        }
    }
}