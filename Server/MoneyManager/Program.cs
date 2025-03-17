using AutoMapper;
using BLL.Interfaces.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManager.BLL.Mappings;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.DAL.Database;
using MoneyManager.DAL.Interfaces;
using MoneyManager.WEB.Mappings;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// TODO: Transfer to DAL
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

