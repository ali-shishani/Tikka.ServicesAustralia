using Tikka.ServicesAustralia;
using Tikka.ServicesAustralia.Core;
using Tikka.ServicesAustralia.Core.Middlewares;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Tikka Configurations
builder.Services.RegisterConfigurations(builder.Configuration);

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AgedCare Microservice API",
        Version = "v1"
    });

    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

// Tikka Services
builder.Services.RegisterTikkaServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Swagger in Development
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgedCare Microservice Api V1"));
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// Tikka Jwt Middleware
//app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();