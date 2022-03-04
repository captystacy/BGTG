using POS.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));
DependencyContainer.CalendarPlan(builder.Services);
DependencyContainer.Estimate(builder.Services);

builder.WebHost.ConfigureKestrel(x => x.ListenAnyIP(5000));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html"); ;

app.Run();
