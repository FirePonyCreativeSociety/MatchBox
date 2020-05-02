using AutoMapper;
using MatchBox.Configuration;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models.Mapping;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace MatchBox
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
            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection(MatchBoxConfiguration.AppSettingsSectionName);
            services.Configure<MatchBoxConfiguration>(appSettingsSection);
            var mbCfg = appSettingsSection.Get<MatchBoxConfiguration>();
            services.AddSingleton<MatchBoxConfiguration>(mbCfg);
            services.AddSingleton<EmailConfiguration>(mbCfg.Email);
            services.AddSingleton<JwtConfiguration>(mbCfg.Jwt);
            services.AddSingleton<PasswordConfiguration>(mbCfg.Password);
            services.AddSingleton<UserConfiguration>(mbCfg.User);

            // Configure jwt authentication            
            // TODO: this is not the ideal place to check... or is it?
            if (string.IsNullOrWhiteSpace(mbCfg.Jwt.IssuerSigningKey))
                throw new Exception($"Unspecified Jwt IssuerSigningKey value in configuration.");
                        
            var key = Encoding.ASCII.GetBytes(mbCfg.Jwt.IssuerSigningKey);
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,                    
                };
            });
            
            // Auto mapper
            services.AddAutoMapper(typeof(APIAutoMapProfile));

            // Database
            services.AddDbContext<MatchBoxDbContext>(opt =>
            {
                var connStr = Configuration.GetConnectionString(MatchBoxDbContext.DbConnectionName);

                // TODO: temporary. This check me pick an RDBMS type
                if (connStr.Contains("server", System.StringComparison.OrdinalIgnoreCase) && (connStr.Contains("database", System.StringComparison.OrdinalIgnoreCase) || connStr.Contains("initial catalog", System.StringComparison.OrdinalIgnoreCase)))
                    opt.UseSqlServer(connStr);                
                else
                    opt.UseSqlite(connStr); 
            });

            services.AddIdentity<DbUser, DbRole>(options =>
                    {
                        options.Password.RequireLowercase = mbCfg.Password.RequireLowercase;
                        options.Password.RequireUppercase = mbCfg.Password.RequireUppercase;
                        options.Password.RequireNonAlphanumeric = mbCfg.Password.RequireNonAlphanumeric;
                        options.Password.RequireDigit = mbCfg.Password.RequireDigit;

                        options.Lockout.AllowedForNewUsers = true;
                        options.User.RequireUniqueEmail = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(mbCfg.User.LockoutDurationInMinutes);
                        options.Lockout.MaxFailedAccessAttempts = mbCfg.User.MaxFailedAccessAttempts;                                               
                        options.User.AllowedUserNameCharacters = mbCfg.User.AllowedUserNameCharacters;
                    })                
                    .AddEntityFrameworkStores<MatchBoxDbContext>()
                    .AddDefaultTokenProviders();

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc(name: "v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MatchBox API", Version = "v1" });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: 'bearer {token}'",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                c.OperationFilter<AuthenticationRequirementsOperationFilter>();
            });

            services.AddTransient<IJwtProducer, JwtProducer>();
            services.AddTransient<IEmailSender, EmailSender>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "MatchBox API v1");
            });

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}