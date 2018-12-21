using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BOS.SampleApp.Core.Interfaces;
using BOS.SampleApp.Infrastructure.Data;
using BOS.SampleApp.Web.HttpClients;
using BOS.SampleApp.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BOS.SampleApp.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            string connectionString = Configuration.GetConnectionString("SampleApp");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<DAMSContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IDAMSRepository, DAMSRepository>();

            // Configure Email Sender
            services.AddTransient<IEmailSender>(e => new EmailSender(Configuration["SendGrid:From"], Configuration["SendGrid:ApiKey"]));

            // Setup HttpClients with base addresses to BOS Apis that we need. Preconfigure each request with 
            //  the API Key bearer token.
            services.AddHttpClient<IAuthClient, AuthClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BOS:AuthUrl"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["BOS:ApiKey"]);
            });

            services.AddHttpClient<IDemographicsClient, DemographicsClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BOS:DemographicsUrl"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["BOS:ApiKey"]);
            });

            services.AddHttpClient<IDAMSClient, DAMSClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BOS:DAMSUrl"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["BOS:ApiKey"]);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(c =>
            {
                c.LoginPath = $"/Identity/Account/Login";
                c.LogoutPath = $"/Identity/Account/Logout";
            });
            services.AddMvc().AddFeatureFolders().AddAreaFeatureFolders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
            routes.MapRoute(
            name: "areaRoute",
            template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
