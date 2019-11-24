using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.Model.Client;
using MoneyManager.WEB.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.WEB.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IMapper _mapper;
        public TransactionTypeController(ITransactionTypeService transactionTypeService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionTypeService = transactionTypeService;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionTypeModel>> GetAll()
        {
            var transactions = await _transactionTypeService.GetAll();
            return _mapper.Map<IEnumerable<TransactionTypeModel>>(transactions);
        }

        [HttpPut]
        public async Task<TransactionTypeModel> Add([FromForm] string name, [FromForm] string extension, [FromForm]IFormFile file)
        {
            var res = await _transactionTypeService.Add(name, extension, file);
            return _mapper.Map<TransactionTypeModel>(res);   
        }

        [HttpPatch]
        public async Task Update(NewTransactionTypeData type)
        {
            await _transactionTypeService.Update(type.Id, type.Name, type.Extension, type.File);
        }

        [HttpDelete]
        public async Task Delete(Guid id) =>
            await _transactionTypeService.Delete(id);
    }
}