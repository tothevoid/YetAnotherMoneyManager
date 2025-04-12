using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MoneyManager.Infrastructure.Entities.Account;
using MoneyManager.Infrastructure.Entities.Currency;
using MoneyManager.Infrastructure.Interfaces.Base;
using MoneyManager.Infrastructure.Data;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Database
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }

        private readonly List<Func<Task>> _commands;

        private static bool _isDataInitialized;

        public MongoContext(IConfiguration configuration)
        {
            _commands = new List<Func<Task>>();
            RegisterConventions();
            var mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            Database = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("DatabaseName").Value);

            //TODO: fix that workaround
            if (!_isDataInitialized)
            {
                InitializeDefaultData();
            }
        }

        private void InitializeDefaultData()
        {
            var results = new bool[]
            {
                InitData(GetCollection<Currency>(nameof(Currency)), new CurrencyGenerator().Generate()),
                InitData(GetCollection<AccountType>(nameof(AccountType)), new AccountTypeGenerator().Generate())
            };

            if (results.Any(result => result))
            {
                SaveChanges();
            }

            _isDataInitialized = true;
        }

        private bool InitData<TEntity>(IMongoCollection<TEntity> collection, IEnumerable<TEntity> data)
            where TEntity : BaseEntity
        {
            if (collection.Find(FilterDefinition<TEntity>.Empty).Any())
            {
                return false;
            }

            collection.InsertMany(data);

            return true;
        }

        private void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("netcore conventions", pack, t => true);
        }

        public int SaveChanges()
        {
            var qtd = _commands.Count;
            foreach (var command in _commands)
            {
                command();
            }

            _commands.Clear();
            return qtd;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task AddCommand(Func<Task> func)
        {
            _commands.Add(func);
            return Task.CompletedTask;
        }
    }
}
