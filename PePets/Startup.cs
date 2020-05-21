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

            services.AddAuthentication()
                .AddGoogle("Google", googleOptions =>
                {
                    googleOptions.ClientId = Configuration.GetSection("GoogleOAuth").GetValue<string>("ClientId");
                    googleOptions.ClientSecret = Configuration.GetSection("GoogleOAuth").GetValue<string>("ClientSecret");
                    googleOptions.CallbackPath = new PathString("/ExternalLoginCallback");
                    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    googleOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);
                    googleOptions.Scope.Add("profile");
                    googleOptions.Events.OnCreatingTicket = (context) =>
                    {
                        JObject userInfo = JObject.Parse(context.User.ToString());

                        // Получаем URL аватарки пользователя и вносим это в утверждение
                        context.Identity.AddClaim(new Claim("picture", userInfo.GetValue("picture").ToString()));

                        return Task.CompletedTask;
                    };
                })
                .AddVkontakte("VK", VkOptions =>
                {
                    VkOptions.ClientId = Configuration.GetSection("VkOAuth").GetValue<string>("ClientId");
                    VkOptions.ClientSecret = Configuration.GetSection("VkOAuth").GetValue<string>("ClientSecret");
                    VkOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    VkOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);
                    VkOptions.CallbackPath = new PathString("/signin-vk");

                    VkOptions.Scope.Add("email");
                    VkOptions.Scope.Add("photos");

                    VkOptions.Events.OnCreatingTicket = (context) =>
                    {
                        JObject userInfo = JObject.Parse(context.User.ToString());

                        // Получаем URL аватарки пользователя и вносим это в утверждение
                        context.Identity.AddClaim(new Claim("picture", userInfo.GetValue("photo_rec").ToString()));

                        return Task.CompletedTask;
                    };
                })
                /*
                .AddOAuth("VK", options =>
                {
                    options.ClientId = Configuration.GetSection("VkOAuth").GetValue<string>("ClientId");
                    options.ClientSecret = Configuration.GetSection("VkOAuth").GetValue<string>("ClientSecret");
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.BackchannelTimeout = TimeSpan.FromSeconds(60);

                    options.CallbackPath = "/signin-vk";
                    options.AuthorizationEndpoint = "https://oauth.vk.com/authorize?display=page";
                    options.TokenEndpoint = "https://oauth.vk.com/access_token";

                    options.Scope.Add("email");

                    options.Events.OnCreatingTicket = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("access_token", context.AccessToken));
                        return Task.CompletedTask;
                    };
                })
                
                .AddVK("VK", VkOptions =>
                {
                    VkOptions.ClientId = Configuration.GetSection("VkOAuth").GetValue<string>("ClientId");
                    VkOptions.ClientSecret = Configuration.GetSection("VkOAuth").GetValue<string>("ClientSecret");
                    VkOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    VkOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);
                    //VkOptions.CallbackPath = new PathString("/ExternalLoginCallback");
                    VkOptions.CallbackPath = "/signin-vk";

                    VkOptions.Scope.Add("email");
                    
                    
                    VkOptions.Fields.Add("uid");
                    VkOptions.Fields.Add("first_name");
                    VkOptions.Fields.Add("last_name");

                    VkOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
                    VkOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    VkOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
                    VkOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
                    
                })*/;
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
