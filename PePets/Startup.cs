using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PePets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;

namespace PePets
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
            services.AddDbContext<PePetsDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("PePetsDbContext")));
            services.AddTransient<AdvertRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<BreedRepository>();
            services.AddTransient<TypeRepository>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PePetsDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle("Google", googleOptions => {
                googleOptions.ClientId = "203642452482-pr55ub8jdb5s2aj4485nd80aa0b4vu70.apps.googleusercontent.com";
                googleOptions.ClientSecret = "O5FtzBJDRSXk-C5mSIEgss9n";
                googleOptions.CallbackPath = new PathString("/GoogleLoginCallback");
                googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                googleOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
