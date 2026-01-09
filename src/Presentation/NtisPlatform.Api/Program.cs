using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using NtisPlatform.Api.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Register all services in one place
builder.Services.AddAllServices(builder.Configuration);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization();
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("hi"),
    new CultureInfo("mr"),
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new AcceptLanguageHeaderRequestCultureProvider(), // for Swagger/Postman
        new CookieRequestCultureProvider()                // for UI cookie selection
    };
});

var app = builder.Build();

// Global exception handling
app.UseMiddleware<NtisPlatform.Api.Middleware.GlobalExceptionHandlerMiddleware>();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS must come before authentication
app.UseCors("AllowAll");

// Rate limiting - TEMPORARILY DISABLED FOR DEVELOPMENT
// app.UseRateLimiter();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
