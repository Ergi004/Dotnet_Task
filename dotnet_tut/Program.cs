using Microsoft.EntityFrameworkCore;
using dotnet_tut.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version   = "v1",
        Title     = "Transactions API",
        Description = "An API to manage financial transactions",
        Contact   = new OpenApiContact
        {
            Name  = "Ergi Lama",
            Email = "ergi@example.com"
        },
        License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
    });

    var xmlFile   = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath   = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.TagActionsBy(api => api.GroupName ?? api.HttpMethod);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Description = "Enter ‘Bearer {token}’",
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme, Id = "Bearer"
            }},
            Array.Empty<string>()
        }
    });
});


builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
          .WithOrigins("http://localhost:3000")   
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions API v1");
    c.RoutePrefix = "swagger"; 
});

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();

app.UseRouting();

app.MapControllers();




app.Run();
