using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Messages;
using TickerQ.DependencyInjection;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Extensions;
using MoneyManager.Infrastructure.Extensions;
using MoneyManager.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddClientCors(builder.Configuration);

builder.Services.AddDatabaseConnection(builder.Configuration);

builder.Services.AddTickerQ();

builder.Services.AddMinioConfiguration(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddMvc();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddMappings();

var app = builder.Build();

app.MigrateDatabase();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection()

app.MapHub<ServerMessagesHub>("/messages");

app.Map("/Error", (HttpContext context) =>
{
    return Results.Problem("Something went wrong");
});
    
app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseTickerQ();

app.Run();