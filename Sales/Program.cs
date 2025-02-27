using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Sales.Configurations;
using Sales.Services;
using Sales.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales API",
        Version = "v1",
        Description = "API para registrar, buscar e atualizar vendas"
    });
    options.SchemaFilter<GenericResponseSchemaFilter>();
});

builder.Services.AddSingleton<ISalesService, SalesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API v1");
        options.RoutePrefix = "api-docs";
    });
}

app.MapControllers();
app.Run();