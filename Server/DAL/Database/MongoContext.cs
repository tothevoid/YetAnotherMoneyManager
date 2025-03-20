using MoneyManager.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MoneyManager.DAL.Entities;
using MoneyManager.Data;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

namespace MoneyManager.DAL.Database
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
            var currencyCollection = Database.GetCollection<Currency>(nameof(Currency));
            var defaultCurrencies = new DefaultCurrencies().GetCurrencies();
            currencyCollection.InsertMany(defaultCurrencies);

            SaveChanges();
            _isDataInitialized = true;
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
