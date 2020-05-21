using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using PePets.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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

            services.AddAuthentication().AddGoogle("Google", googleOptions =>
            {
                googleOptions.ClientId = Configuration.GetSection("GoogleOAuth").GetValue<string>("ClientId");
                googleOptions.ClientSecret = Configuration.GetSection("GoogleOAuth").GetValue<string>("ClientSecret");
                googleOptions.CallbackPath = new PathString("/GoogleLoginCallback");
                googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                googleOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);
                googleOptions.Scope.Add("profile");
                googleOptions.Events.OnCreatingTicket = (context) =>
                {
                    JObject userInfo = JObject.Parse(context.User.ToString());

                    // Получаем URL аватарки пользователя и вносим это в утверждение
                    context.Identity.AddClaim(new Claim("picture", userInfo.GetValue("picture").ToString()));

                    // Вносим утверждение о подтвержденном email адресе пользователя 
                    context.Identity.AddClaim(new Claim("confirmed_email", userInfo.GetValue("verified_email").ToString()));

                    return Task.CompletedTask;
                };
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
