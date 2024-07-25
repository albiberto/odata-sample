using System.Reflection;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Obama;
using Obama.Domain;
using Serilog;

static IEdmModel GetEdmModel() 
{ 
    var builder = new ODataConventionModelBuilder(); 
            
    builder.EntitySet<Role>("Roles"); 
    builder.EntitySet<Employee>("Employees"); 
        
    return builder.GetEdmModel(); 
}

var name = Assembly.GetExecutingAssembly().GetName().Name;
var model = GetEdmModel();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Information("{solution} building up...", name);

builder.AddLogger();
builder.AddOData(model);
builder.AddOpenAPI();
builder.AddInfrastructure();

Log.Information("{solution} starting up...", name);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseODataBatching(); 
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

Log.Information("{solution} started!", name);

app.Run();