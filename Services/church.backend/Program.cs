using Enerfan.DataBase;
using Enerfan.Services;
using Enerfan.Utilities;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

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

//database
builder.Services.AddSingleton<StationDataBase>();
builder.Services.AddSingleton<QrDataBase>();
builder.Services.AddSingleton<AccessDataBase>();
builder.Services.AddSingleton<PhotoProfileDataBase>();
builder.Services.AddSingleton<PromotionDataBase>();
builder.Services.AddSingleton<InvoiceDataBase>();

//services
builder.Services.AddSingleton<StationServices>();
builder.Services.AddSingleton<QrServices>();
builder.Services.AddSingleton<AccessServices>();
builder.Services.AddSingleton<PromotionServices>();
builder.Services.AddSingleton<PhotoProfileServices>();
builder.Services.AddSingleton<InvoiceService>();

//utils
builder.Services.AddSingleton<xmlTools>();
builder.Services.AddSingleton<Utils>();
builder.Services.AddSingleton<PasswordMaker>();
builder.Services.AddSingleton<NullValues>();
builder.Services.AddSingleton<NipValidator>();
builder.Services.AddSingleton<ImageUtils>();
builder.Services.AddSingleton<LocationUtil>();

//jwt
builder.Services.AddSingleton(provider => new JwtService("Enerfanmnjhbvgfcrdexcfrvgbhnjmhgvfrcvgbhnyv234dfg", "enerfan_issuer"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var config = app.Configuration;
config["ConnectionStrings:db"] = DotNetEnv.Env.GetString("db_connection");
config["ConnectionStrings:invoice"] = DotNetEnv.Env.GetString("invoice");
config["ConnectionStrings:sepomex"] = DotNetEnv.Env.GetString("sepomex");
config["Invoice:url"] = DotNetEnv.Env.GetString("invoiceUrlBase");
config["Invoice:user"] = DotNetEnv.Env.GetString("invoiceUser");
config["Invoice:password"] = DotNetEnv.Env.GetString("invoicePassword");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
