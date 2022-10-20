using checkout.com.api.BusinessLogic;
using checkout.com.api.Data;
using checkout.com.api.Database;
using checkout.com.api.Integrations;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICardVerification, CardVerification>();
builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IBankRequest, BankRequest>();
builder.Services.AddTransient<IBusinessLogicLayer, BusinessLogicLayer>();


builder.Services.AddOptions();
var endpoints = configuration.GetSection("EndpointConfig");
var database = configuration.GetSection("DatabaseConnection");
builder.Services.Configure<EndPointConfig>(endpoints);
builder.Services.Configure<DatabaseConnection>(database);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
