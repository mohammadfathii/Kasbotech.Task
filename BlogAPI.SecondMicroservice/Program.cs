using BlogAPI.SecondMicroservice;
using BlogAPI.SecondMicroservice.Data;
using BlogAPI.SecondMicroservice.Services;
using BlogAPI.SecondMicroservice.Services.IServices;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<ICandleService, CandleService>();

var host = builder.Build();
host.Run();
