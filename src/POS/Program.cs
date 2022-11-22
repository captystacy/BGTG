using POS.Infrastructure.DependencyInjection;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

DependencyContainer.Common(builder.Services);
DependencyContainer.Estimate(builder.Services);
DependencyContainer.DurationByLC(builder.Services);
DependencyContainer.CalendarPlan(builder.Services);
DependencyContainer.EnergyAndWater(builder.Services);
DependencyContainer.DurationByTCP(builder.Services);
DependencyContainer.EmployeesNeed(builder.Services);
DependencyContainer.POS(builder.Services);

if (builder.Environment.IsProduction())
{
    if (!int.TryParse(builder.Configuration.GetSection("Port").Value, out var port))
    {
        throw new FormatException("Port from configuration file is not valid.");
    }

    builder.WebHost.ConfigureKestrel(x => x.ListenAnyIP(port));
}

var app = builder.Build();

var ruCulture = new CultureInfo("ru-RU") { NumberFormat = { NumberDecimalSeparator = "." } };
CultureInfo.DefaultThreadCurrentCulture = ruCulture;
CultureInfo.DefaultThreadCurrentUICulture = ruCulture;

var mapper = app.Services.GetRequiredService<AutoMapper.IConfigurationProvider>();

if (builder.Environment.IsDevelopment())
{
    mapper.AssertConfigurationIsValid();
}
else
{
    mapper.CompileMappings();
}

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapFallbackToFile("index.html");

app.Run();