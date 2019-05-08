using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace VcwBackend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            const string securityKeyString = "KoHzAdIhOsSeIn is My secret Key";
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKeyString));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                // Validate the JWT Issuer (iss) claim  
                ValidateIssuer = false,
                //                ValidIssuer = Configuration.GetSection("AppConfiguration:SiteUrl").Value,


                // Validate the JWT Audience (aud) claim  
                ValidateAudience = false,
                //                ValidAudience = Configuration.GetSection("AppConfiguration:SiteUrl").Value,

                // Validate the token expiry  
                ValidateLifetime = false,
                //                ValidAudience = false,
                ValidateActor = false,
                ValidateTokenReplay = false
                //                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });


            // Add framework services.
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseCors("Cors");
            app.UseMvc();
        }
    }
}