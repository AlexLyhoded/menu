using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace menu.Swagger
{
    public class DisplaySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            foreach (var property in context.Type.GetProperties())
            {
                var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayAttribute != null)
                {
                    schema.Properties[property.Name].Description = displayAttribute.Name;
                }
            }
        }
    }
}
