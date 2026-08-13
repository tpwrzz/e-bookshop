using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi;
using System.Collections.Generic;
using System.Linq;

namespace Bookshop.SharedKernel
{
    public static class SwaggerForwardedPrefixExtensions
    {
        public static void UseSwaggerWithForwardedPrefix(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((document, httpRequest) =>
                {
                    var forwardedPrefix = httpRequest.Headers["X-Forwarded-Prefix"].FirstOrDefault();
                    Console.WriteLine($"[DEBUG] X-Forwarded-Prefix = '{forwardedPrefix}'");
                    if (!string.IsNullOrEmpty(forwardedPrefix))
                    {
                        document.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = forwardedPrefix }
                        };
                    }
                });
            });
        }
    }
}