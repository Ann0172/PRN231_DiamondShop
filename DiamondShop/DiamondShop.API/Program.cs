using DiamondShop.Api.Middlewares;
using DiamondShop.API.Extensions;
using DiamondShop.Repository.Extensions;
using DiamondShop.Service.Extensions;
using System.Configuration;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiamondShop.Repository.Enums;
using DiamondShop.Service.Interfaces;
using DiamondShop.Service.Services;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);
var payOs = new PayOS(builder.Configuration["PayOSConfig:PAYOS_CLIENT_ID"]!,
    builder.Configuration["PayOSConfig:PAYOS_API_KEY"]!,
    builder.Configuration["PayOSConfig:PAYOS_CHECKSUM_KEY"]!);
builder.Services.AddSingleton(payOs);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
// Add services to the container.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{

    containerBuilder.AddRepositoriesDependency();
    containerBuilder.AddServicesDependency(builder.Configuration);
});
builder.Services.AddApiDependencies(builder.Configuration);

builder.Services.AddScoped<IOrderService, OrderService>();
//builder.ConfigureAutofacContainer();

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\diamondshop-ea47a-firebase-adminsdk-6re19-586d0ab870.json");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
