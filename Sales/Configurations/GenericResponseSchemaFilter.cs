using Microsoft.OpenApi.Models;
using Sales.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace Sales.Configurations;

[ExcludeFromCodeCoverage]
public class GenericResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsGenericType && context.Type.GetGenericTypeDefinition() == typeof(Response<>))
        {
            var dataType = context.Type.GetGenericArguments()[0];
            schema.Properties["data"] = context.SchemaGenerator.GenerateSchema(dataType, context.SchemaRepository);
            schema.Properties["errors"] = new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string" } };
        }
    }
}
