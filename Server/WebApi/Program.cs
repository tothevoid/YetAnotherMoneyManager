using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManager.Application.Integrations.Stock;
using MoneyManager.Application.Mappings;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Services.Accounts;
using MoneyManager.Application.Services.Brokers;
using MoneyManager.Application.Services.Currencies;
using MoneyManager.Application.Services.Deposits;
using MoneyManager.Application.Services.Securities;
using MoneyManager.Application.Services.Transactions;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.WebApi.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// TODO: Transfer to Infrastructure solution
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

var clientUrl = builder.Configuration.GetSection("Client").GetSection("Url").Value;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(clientUrl)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITransactionsService, TransactionsService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IDepositService, DepositService>();
builder.Services.AddTransient<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddTransient<ICurrencyService, CurrencyService>();
builder.Services.AddTransient<IAccountTypeService, AccountTypeService>();
builder.Services.AddTransient<IBrokerAccountSecurityService, BrokerAccountSecurityService>();
builder.Services.AddTransient<IBrokerAccountService, BrokerAccountService>();
builder.Services.AddTransient<IBrokerAccountTypeService, BrokerAccountTypeService>();
builder.Services.AddTransient<IBrokerService, BrokerService>();
builder.Services.AddTransient<ISecurityService, SecurityService>();
builder.Services.AddTransient<ISecurityTransactionService, SecurityTransactionService>();
builder.Services.AddTransient<ISecurityTypeService, SecurityTypeService>();

//TODO: make factory
builder.Services.AddTransient<IStockConnector, MoexConnector>();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<DTOToEntityProfile>();
    cfg.AddProfile<ViewToDTOProfile>();
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowReactApp");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection()

app.UseRouting();
app.MapControllers();

app.Run();