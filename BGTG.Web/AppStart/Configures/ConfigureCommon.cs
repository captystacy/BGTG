using BGTG.Web.AppStart.ConfigureServices;
using BGTG.Web.Middlewares;
using Calabonga.Microservices.Web.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BGTG.Web.AppStart.Configures
{
    public static class ConfigureCommon
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider mapper)
        {
            if (env.IsDevelopment())
            {
                mapper.AssertConfigurationIsValid();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                mapper.CompileMappings();

                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            app.UseResponseCaching();
            app.UseETagger();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseSwagger();
            app.UseSwaggerUI(ConfigureServicesSwagger.SwaggerSettings);

            // Singleton setup for User Identity
            UserIdentity.Instance.Configure(app.ApplicationServices.GetService<IHttpContextAccessor>());
        }
    }
}
