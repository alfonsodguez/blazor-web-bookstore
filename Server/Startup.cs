using bookstore.Server.Models;
using bookstore.Server.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;

namespace bookstore.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IClienteEmail, ClienteCorreoMAILJET>();
            services.Configure<ConfigMailjet>((opciones) => {
                opciones.ServerName = Configuration.GetSection("SMTPMailJet").GetValue<string>("ServerName");
                opciones.APIKey = Configuration.GetSection("SMTPMailJet").GetValue<string>("APIKey");
                opciones.SecretKey = Configuration.GetSection("SMTPMailJet").GetValue<string>("SecretKey");
            });

            string cadenaConexion = Configuration.GetConnectionString("AgapeaDBConnectionString");
            string ensamblado = Assembly.GetExecutingAssembly().GetName().Name;
            services.AddDbContext<AplicacionDBContext>((DbContextOptionsBuilder opciones) => {
                opciones.UseSqlServer(cadenaConexion, (SqlServerDbContextOptionsBuilder opcion) => opcion.MigrationsAssembly(ensamblado));
            });

            services.AddIdentity<ClienteIdentity, IdentityRole>((IdentityOptions opciones) => {
                opciones.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 6,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = true
                };
                opciones.SignIn = new SignInOptions
                {
                    RequireConfirmedEmail = true
                };
                opciones.Lockout = new LockoutOptions
                {
                    AllowedForNewUsers = false,
                    MaxFailedAccessAttempts = 3,
                    DefaultLockoutTimeSpan = System.TimeSpan.FromHours(5)
                };
                opciones.User = new UserOptions
                {
                    RequireUniqueEmail = true
                };
            })
            .AddEntityFrameworkStores<AplicacionDBContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ClienteIdentity>>(TokenOptions.DefaultProvider);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((JwtBearerOptions opciones) => {
                    opciones.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JWT:clave"]))
                    };
                });

            services.AddControllersWithViews().AddJsonOptions((JsonOptions opciones) => { opciones.JsonSerializerOptions.PropertyNamingPolicy = null; });
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
