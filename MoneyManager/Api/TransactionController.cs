using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MoneyManager.Model;
using System.Collections.Generic;
using System;

namespace MoneyManager.Api
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly Random _random;
        public TransactionController()
        {   
            _random = new Random();
        }

        [HttpGet]
        public IEnumerable<Transaction> GetAll()
        {
            return Enumerable.Range(0, 10).Select((x,i)=>GenerateTransaction(i));
        }

        [HttpPut]
        public void Add(Transaction transaction)
        {
            
        }

        [HttpDelete]
        public void Delete(Guid id)
        {

        }

        public Transaction GenerateTransaction(int index) =>
            new Transaction{
                Name = "test",
                MoneyQuantity = _random.Next(-1000,1000),
                Date = DateTime.Now.AddDays(-1*index),  
                Description = "empty",
                Type = 0,
                Id = Guid.NewGuid()
            };
    }
}