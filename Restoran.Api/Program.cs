using Microsoft.EntityFrameworkCore;
using Restoran.Business.Services;
using Restoran.Data;
using Restoran.Data.Entities;
using Restoran.Data.Repository;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RestoranDbContext>(options => options.UseMySql
(builder.Configuration.GetConnectionString("DefaultConnection"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped<Restoran.Data.Repository.IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<Restoran.Business.Services.IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

 

 
app.MapControllers();
app.Run();

 
