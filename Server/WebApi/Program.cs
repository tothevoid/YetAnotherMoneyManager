using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManager.Application.Integrations.Stock;
using MoneyManager.Application.Mappings;
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
using Microsoft.Extensions.Configuration;
using MoneyManager.Infrastructure.Interfaces.Messages;
using MoneyManager.Infrastructure.Messages;
using Minio;
using MoneyManager.Application.Integrations.Currency;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Services.FileStorage;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Interfaces.Integrations.Currency;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Services.Crypto;
using MoneyManager.Application.Services.User;
using MoneyManager.Application.Services.Dashboard;
using MoneyManager.Application.Services.Debts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var clientUrl = builder.Configuration.GetSection("Client").GetSection("Url").Value;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(clientUrl)
                .AllowAnyMethod()
                .AllowCredentials()
            .AllowAnyHeader();
        });
});


var dbConnection = builder.Configuration.GetSection("DB").GetSection("ConnectionString").Value;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(dbConnection));


var fileStorageSection = builder.Configuration.GetSection("FileStorage");

var host = fileStorageSection.GetSection("Host").Value;
var user = fileStorageSection.GetSection("User").Value;
var password = fileStorageSection.GetSection("Password").Value;


builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(host, 9000)
    .WithSSL(false)
    .WithCredentials(user, password)
    .Build());

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddMvc();
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
builder.Services.AddTransient<IDividendService, DividendService>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();
builder.Services.AddTransient<IDebtService, DebtService>();
builder.Services.AddTransient<IDebtPaymentService, DebtPaymentService>();
builder.Services.AddTransient<IDividendPaymentService, DividendPaymentService>();

builder.Services.AddTransient<ICryptoAccountService, CryptoAccountService>();
builder.Services.AddTransient<ICryptoAccountCryptocurrencyService, CryptoAccountCryptocurrencyService>();
builder.Services.AddTransient<ICryptocurrencyService, CryptocurrencyService>();
builder.Services.AddTransient<ICryptoProviderService, CryptoProviderService>();

builder.Services.AddScoped<IServerNotifier, ServerNotifier>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

//TODO: make factory
//TODO: possible change AddTransient to AddSingleton
builder.Services.AddTransient<IStockConnector, MoexConnector>();
builder.Services.AddTransient<ICurrencyGrabber, CbrCurrencyGrabber>();

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

app.MapHub<ServerMessagesHub>("/messages");
    
app.UseRouting();
app.MapControllers();

app.Run();