using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Server
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
            services.AddControllers(op => {
                op.RespectBrowserAcceptHeader = true;
            });
            services.AddHostedService<ExchageRatesBackgroundService>();
            services.AddScoped<ICommunicationServices,CommunicationServices>();
            
            // Add Database Context Factory
            string connectionStr = Configuration.GetValue<string>("DatabaseOptions:Connection");
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(connectionStr, b =>
                {
                    b.EnableRetryOnFailure(5);
                });
            });

            // add asp.net identity for application users
            services.AddIdentity<User, IdentityRole>(op =>
            {
                // password requirement configuration
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireDigit = false;
                op.Password.RequiredLength = 6;
                op.Password.RequireLowercase = false;
                op.Password.RequireUppercase = false;
                // account lockout configuration
                op.Lockout.AllowedForNewUsers = false;
                // user account configuration
                op.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

            var jwtSettings = Configuration.GetSection("JWTSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(config => {
                config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
