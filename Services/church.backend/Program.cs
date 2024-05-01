using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<accessDB>();
builder.Services.AddSingleton<rolesDB>();
builder.Services.AddSingleton<cathedralDB>();
builder.Services.AddSingleton<citiesDB>();
builder.Services.AddSingleton<civilStatusDB>();
builder.Services.AddSingleton<daysDB>();
builder.Services.AddSingleton<discountDB>();
builder.Services.AddSingleton<AccessServices>();
builder.Services.AddSingleton<RolesServices>();
builder.Services.AddSingleton<CathedralServices>();
builder.Services.AddSingleton<CitiesServices>();
builder.Services.AddSingleton<CivilStatusServices>();
builder.Services.AddSingleton<DaysServices>();
builder.Services.AddSingleton<DiscountServices>();
builder.Services.AddSingleton(provider => new JwtService("CHURCHmnjhbvgfcrdexcfrvgbhnjmhgvfrcvgbhnyv234dfg", "church_issuer"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API",
        Description = "Documentation of RestServives",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JsonWebToken",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Coloca **SOLO** tu JWT token en el recuadro de abajo",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
