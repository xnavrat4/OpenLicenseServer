using System.Text;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using OpenLicenseServerDAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenLicenseServerAPI;
using static OpenLicenseServerAPI.DataCrypter;
using ConfigurationManager = System.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("OLSConnectionString");
var privateKey = builder.Configuration.GetSection("PrivateKey").Value;

var bootstrapper = new Bootstrapper(connString, privateKey);
builder.Logging.AddConsole();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(autofacBuilder => bootstrapper.PrepareBuilder(autofacBuilder));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "OpenLicenseServer", 
        Version = "v1" 
    });
});
builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });


using var app = builder.Build();

bootstrapper.PrepareDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.Run();
