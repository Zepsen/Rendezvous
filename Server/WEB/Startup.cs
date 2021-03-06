﻿using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WEB.Infrastructure.Extensions;
using WEB.Infrastructure.Hubs;
using WEB.Infrastructure.Middleware;

namespace WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddAppAuthServices();
            services.AddAppIdentityServices();
            services.AddAppInternalServices();

            services.AddResponseCompression();
            services.AddAutoMapper();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:8091")
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }));

            services.AddSignalR().AddMessagePackProtocol();
            services.AddMvc();
            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
                app.UseHsts();   
            }
            app.UseAuthentication();
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            //app.UseSignalR(routes => { routes.MapHub<ApplicationHub>("/hub"); });

            app.UseMvc();
        }
    }
}
