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
using PePets.Services;
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
            services.AddTransient<PostRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<BreedRepository>();
            services.AddTransient<TypeRepository>();
            services.AddSingleton<SearchService>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;   // минимальная длина
                options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                options.Password.RequireDigit = true; // требуются ли цифры
            })
                .AddEntityFrameworkStores<PePetsDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle("Google", googleOptions =>
                {
                    googleOptions.ClientId = Configuration["GoogleOAuth:ClientId"];
                    googleOptions.ClientSecret = Configuration["GoogleOAuth:ClientSecret"];
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
                .AddFacebook("Facebook", FacebookOptions =>
                {
                    FacebookOptions.ClientId = Configuration["FacebookOAuth:ClientId"];
                    FacebookOptions.ClientSecret = Configuration["FacebookOAuth:ClientSecret"];
                    FacebookOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    FacebookOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);

                    FacebookOptions.Fields.Add("picture.type(large)");
                    FacebookOptions.Events.OnCreatingTicket = (context) =>
                    {
                        JObject userInfo = JObject.Parse(context.User.ToString());
                        JToken pictureUrl = userInfo.SelectToken("picture").SelectToken("data").SelectToken("url");

                        // Получаем URL аватарки пользователя и вносим это в утверждение
                        context.Identity.AddClaim(new Claim("picture", pictureUrl.Value<string>()));

                        return Task.CompletedTask;
                    };
                })
                .AddOdnoklassniki("Odnoklassniki", OdnoklassnikiOptions => 
                {
                    OdnoklassnikiOptions.ClientId = Configuration["OdnoklassnikiOAuth:ClientId"];
                    OdnoklassnikiOptions.ClientSecret = Configuration["OdnoklassnikiOAuth:ClientSecret"];
                    OdnoklassnikiOptions.PublicSecret = Configuration["OdnoklassnikiOAuth:ApplicationKey"];
                    OdnoklassnikiOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    OdnoklassnikiOptions.BackchannelTimeout = TimeSpan.FromSeconds(60);

                    OdnoklassnikiOptions.Scope.Add("GET_EMAIL");

                    OdnoklassnikiOptions.Events.OnCreatingTicket = (context) =>
                    {
                        JObject userInfo = JObject.Parse(context.User.ToString());

                        // Получаем URL аватарки пользователя и вносим это в утверждение
                        context.Identity.AddClaim(new Claim("picture", userInfo.GetValue("pic_3").ToString()));

                        return Task.CompletedTask;
                    };
                })
                .AddVkontakte("VK", VkOptions =>
                {
                    VkOptions.ClientId = Configuration["VkOAuth:ClientId"];
                    VkOptions.ClientSecret = Configuration["VkOAuth:ClientSecret"];
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
