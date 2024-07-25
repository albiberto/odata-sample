using System.Reflection;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Obama.Domain;
using Obama.Infrastructure;
using Obama.Infrastructure.DevSpace;
using Serilog;

namespace Obama;

public static class Bootstrapper
{
    public static void AddOData(this WebApplicationBuilder builder, IEdmModel edmModel)
    {
        builder.Services
            .AddControllers()
            .AddOData(options => options
                .EnableQueryFeatures(100)
                .AddRouteComponents("odata", edmModel, new DefaultODataBatchHandler())
            );
    }

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    public static void AddOpenAPI(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = Assembly.GetExecutingAssembly().GetName().Name, Version = "v1" });
        });
        
        builder.AddFormatters();
    }

    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

        if (!builder.Environment.IsProduction())
        {
            builder.Services.AddDevSeeder();
            builder.Services.AddMigration<ObamaContext, ObamaDevContextSeeder>();

            return;
        }

        builder.Services.AddMigration<ObamaContext>();
    }

    public static void AddLogger(this WebApplicationBuilder builder) => builder.Services.AddLogging(b => b.AddSerilog(Log.Logger, true));

    private static void AddFormatters(this WebApplicationBuilder builder)
    {
        builder.Services.AddMvcCore(options =>
        {
            foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
            {
                outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            }

            foreach (var inputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
            {
                inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            }
        });
    }
}