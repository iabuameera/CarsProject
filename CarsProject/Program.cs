using Cars.Data.PostgreSQL.Data;
using CarsProject.Cars.CarRepositry;
using CarsProject.Cars.CarService;
using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<, CarDbContext>();
//builder.Services.AddScoped<ICarService>();
//builder.Services.AddScoped<ICarRepositry>();

builder.Services.AddScoped(typeof(ICarService), typeof(CarService));
builder.Services.AddScoped(typeof(ICarRepositry), typeof(CarRepositry));

builder.Services.AddDbContext<CarDbContext>(options =>options.UseNpgsql(
    builder.Configuration.GetConnectionString("WebApiDatabase")));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
