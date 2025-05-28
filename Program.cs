using CryptoQuoteApi.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// <-------------------- Register Application Services -------------------->

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CryptoQuote API",
        Version = "v1"
    });
});

// Configure CORS to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// <-------------------- Register HttpClients -------------------->

// CoinMarketCapClient
builder.Services.AddHttpClient("CoinMarketCap", client =>
{
    client.BaseAddress = new Uri("https://pro-api.coinmarketcap.com/v1/");
    client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", builder.Configuration["CoinMarketCap:ApiKey"]);
});
builder.Services.AddScoped<CoinMarketCapClient>();

// ExchangeRatesApi
builder.Services.AddHttpClient("ExchangeRates", client =>
{
    client.BaseAddress = new Uri("https://api.apilayer.com/exchangerates_data/");
    client.DefaultRequestHeaders.Add("apikey", builder.Configuration["ExchangeRates:ApiKey"]);
});
builder.Services.AddScoped<ExchangeRatesClient>();

// <-------------------- Build and Configure the App -------------------->

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoQuote API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
