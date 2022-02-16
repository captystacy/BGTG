using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Data.DatabaseInitialization
{
    /// <summary>
    /// Database Initializer
    /// </summary>
    public static class DatabaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            await context!.Database.MigrateAsync();

            // TODO: Add your seed data here

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}