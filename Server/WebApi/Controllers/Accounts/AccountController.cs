using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.WebApi.Models.Accounts;

namespace MoneyManager.WebApi.Controllers.Accounts
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

        [HttpPost("GetAll")]
        public async Task<IEnumerable<AccountModel>> GetAll(AccountGetAllConfig getAllConfig)
        {
            var accounts = await _accountService.GetAll(getAllConfig.OnlyActive);
            return _mapper.Map<IEnumerable<AccountModel>>(accounts);
        }

        [HttpPost("GetAllByTypes")]
        public async Task<IEnumerable<AccountModel>> GetAllByTypes(AccountGetAllByTypesConfig getAllConfig)
        {
            var accounts = await _accountService.GetAllByTypes(getAllConfig.TypesIds, getAllConfig.OnlyActive);
            return _mapper.Map<IEnumerable<AccountModel>>(accounts);
        }

        [HttpPut]
        public async Task<Guid> Add(AccountModel account)
        {
            var accountDTO = _mapper.Map<AccountDTO>(account);
            return await _accountService.Add(accountDTO);
        }

        [HttpPatch]
        public async Task Update(AccountModel account)
        {
            var accountDTO = _mapper.Map<AccountDTO>(account);
            await _accountService.Update(accountDTO);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _accountService.Delete(id);

        [HttpPost(nameof(Transfer))]
        public async Task Transfer(AccountTransferModel accountTransfer)
        {
            var transferDto = _mapper.Map<AccountTransferDTO>(accountTransfer);
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