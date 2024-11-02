using DiamondShop.Api.Middlewares;
using DiamondShop.API.Extensions;
using DiamondShop.Repository.Extensions;
using DiamondShop.Service.Extensions;
using System.Configuration;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
// Add services to the container.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{

    containerBuilder.AddRepositoriesDependency();
    containerBuilder.AddServicesDependency(builder.Configuration);
});
builder.Services.AddApiDependencies(builder.Configuration);
//builder.ConfigureAutofacContainer();

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
