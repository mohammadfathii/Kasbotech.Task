using BlogAPI.FirstMicroservice;
using BlogAPI.FirstMicroservice.Data;
using BlogAPI.FirstMicroservice.Services;
using BlogAPI.FirstMicroservice.Services.IServices;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<ICandleService, CandleService>();

var host = builder.Build();
host.Run();
