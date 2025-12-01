
using Microsoft.EntityFrameworkCore;
using Orderflow.API.Middleware;
using Orderflow.Application.Interfaces;
using Orderflow.Application.Services;
using Orderflow.Core.Interfaces;
using Orderflow.Infrastructure.Data;
using Orderflow.Infrastructure.Repositories;
using Orderflow.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<OrderflowDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();

// Services
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ITenantService, TenantService>();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<TenantMiddleware>(); // Middleware para la gestión de tenants

app.Run();