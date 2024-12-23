using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace StorageRoom.Api.Filters
{
    /// <summary>
    /// Фильтр для переноса Swagger-аннотаций из интерфейсов в реализации
    /// </summary>
    public class SwaggerOperationFromInterfaceFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodInfo = context.MethodInfo;

            // Получение методов интерфейсов, которые реализует данный класс
            var interfaceMethods = methodInfo.DeclaringType?.GetInterfaces()
                .SelectMany(i => i.GetMethods())
                .Where(iMethod => iMethod.Name == methodInfo.Name);

            if (interfaceMethods == null) return;

            foreach (var interfaceMethod in interfaceMethods)
            {
                // Копирование атрибута SwaggerOperation из интерфейса
                var swaggerOperationAttribute = interfaceMethod
                    .GetCustomAttributes<Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute>()
                    .FirstOrDefault();

                if (swaggerOperationAttribute != null)
                {
                    operation.Summary = swaggerOperationAttribute.Summary;
                    operation.Description = swaggerOperationAttribute.Description;
                    operation.OperationId = swaggerOperationAttribute.OperationId ?? operation.OperationId;
                }

                // Копирование SwaggerResponse атрибутов
                var swaggerResponseAttributes = interfaceMethod
                    .GetCustomAttributes<Swashbuckle.AspNetCore.Annotations.SwaggerResponseAttribute>();

                foreach (var responseAttribute in swaggerResponseAttributes)
                {
                    var statusCode = responseAttribute.StatusCode.ToString();

                    if (!operation.Responses.ContainsKey(statusCode))
                    {
                        operation.Responses.Add(statusCode, new OpenApiResponse
                        {
                            Description = responseAttribute.Description
                        });
                    }
                    else
                    {
                        operation.Responses[statusCode].Description = responseAttribute.Description;
                    }
                }
            }
        }
    }
}
