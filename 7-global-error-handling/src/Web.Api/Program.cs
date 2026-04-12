using Application;
using Infrastructure;
using Web.Api.Endpoints;
using Web.Api.Infrastructure;
using Web.Api.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

// Since ASP.NET 8.0
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Till ASP.NET 8.0
//app.UseMiddleware<ExceptionHandlingMiddleware>();

// Since ASP.NET 8.0
app.UseExceptionHandler();

app.MapUserEndpoints();

app.Run();
